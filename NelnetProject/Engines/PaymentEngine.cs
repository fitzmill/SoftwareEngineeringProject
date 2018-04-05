using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;
using Core.Interfaces;
using Engines.Utils;

namespace Engines
{
    /// <summary>
    /// Charges payments for users.
    /// </summary>
    public class PaymentEngine : IPaymentEngine
    {
        private static readonly int DAYS_UNTIL_OVERDUE = 7;
        private static readonly int DUE_DAY = 5;

        private IGetUserInfoAccessor getUserInfoAccessor;
        private IGetPaymentInfoAccessor getPaymentInfoAccessor;
        private IChargePaymentAccessor chargePaymentAccessor;
        private ISetTransactionAccessor setTransactionAccessor;

        public PaymentEngine(IGetUserInfoAccessor getUserInfoAccessor, IGetPaymentInfoAccessor getPaymentInfoAccessor, 
            IChargePaymentAccessor chargePaymentAccessor, ISetTransactionAccessor setTransactionAccessor)
        {
            this.getUserInfoAccessor = getUserInfoAccessor;
            this.getPaymentInfoAccessor = getPaymentInfoAccessor;
            this.chargePaymentAccessor = chargePaymentAccessor;
            this.setTransactionAccessor = setTransactionAccessor;
        }

        public IList<Transaction> ChargePayments(List<Transaction> charges, DateTime today)
        {
           return charges.Select(charge =>
           {
               //Create dto to be sent to Payment Spring
               PaymentDTO payment = new PaymentDTO
               {
                   CustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(charge.UserID),
                   Amount = (int) charge.AmountCharged * 1000 //Charge is in cents
               };

               //Charge Payment Spring
               ChargeResultDTO result = chargePaymentAccessor.ChargeCustomer(payment);

               //If charge fails, retry for seven days. After that, the charge is marked as failed.
               ProcessState processState = ProcessState.SUCCESSFUL;
               if (!result.WasSuccessful)
               {
                   int daysOverdue = today.Subtract(charge.DateDue).Days;
                   processState = (daysOverdue >= DAYS_UNTIL_OVERDUE) ? ProcessState.FAILED : ProcessState.RETRYING;
               }

               //Generate result transaction
               Transaction resultTransaction = new Transaction
               {
                   TransactionID = charge.TransactionID,
                   UserID = charge.UserID,
                   AmountCharged = charge.AmountCharged,
                   DateDue = charge.DateDue,
                   DateCharged = today,
                   ProcessState = processState,
                   ReasonFailed = result.ErrorMessage
               };

               //Update transaction entry in DB
               setTransactionAccessor.UpdateTransaction(resultTransaction);

               return resultTransaction;
           }).ToList();
        }

        public IList<Transaction> GeneratePayments(DateTime today) //to be run on the 1st of each month
        {
            //Get all users
            IList<User> users = getUserInfoAccessor.GetAllUsers();

            //Generate all payments that are due this month
            List<Transaction> transactions = users.Where(user => TuitionUtil.IsPaymentDue(user.Plan, today))
                    .Select(user => new Transaction
                    {
                        UserID = user.UserID,
                        AmountCharged = TuitionUtil.GenerateAmountDue(user, 2),
                        DateDue = new DateTime(today.Year, today.Month, DUE_DAY),
                        ProcessState = ProcessState.NOT_YET_CHARGED
                    }).ToList();

            //Store them in the database
            transactions.ForEach(t => setTransactionAccessor.AddTransaction(t));

            return transactions;
        }

        public Transaction CalculateNextPaymentForUser(int userID)
        {
            User user = getUserInfoAccessor.GetUserInfoByID(userID);
        }

    }
}
