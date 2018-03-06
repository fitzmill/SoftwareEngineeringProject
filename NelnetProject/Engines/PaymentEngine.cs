using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;
using Core.Interfaces;

namespace Engines
{
    /// <summary>
    /// Charges payments for users.
    /// </summary>
    public class PaymentEngine : IPaymentEngine
    {
        private static readonly int DAYS_UNTIL_OVERDUE = 7;
        private static readonly int DUE_DAY = 5;
        private static readonly int SEMI_DUE_MONTH_1 = 3;
        private static readonly int SEMI_DUE_MONTH_2 = 9;
        private static readonly int YEAR_DUE_MONTH = 9;

        private static readonly int TUITION_K_6 = 2500;
        private static readonly int TUITION_7_8 = 3750;
        private static readonly int TUITION_9_12 = 5000;

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

        public IList<Transaction> ChargePayments(List<Transaction> charges)
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
                   int daysOverdue = DateTime.Today.Subtract(charge.DateDue).Days;
                   processState = (daysOverdue >= DAYS_UNTIL_OVERDUE) ? ProcessState.FAILED : ProcessState.RETRYING;
               }

               //Generate result transaction
               Transaction resultTransaction = new Transaction
               {
                   TransactionID = charge.TransactionID,
                   UserID = charge.UserID,
                   AmountCharged = charge.AmountCharged,
                   DateDue = charge.DateDue,
                   DateCharged = DateTime.Today,
                   ProcessState = processState,
                   ReasonFailed = result.ErrorMessage
               };

               //Update transaction entry in DB
               setTransactionAccessor.UpdateTransaction(resultTransaction);

               return resultTransaction;
           }).ToList();
        }

        public IList<Transaction> GeneratePayments() //to be run on the 1st of each month
        {
            DateTime today = DateTime.Now;

            //Get all users
            //IList<User> users = getUserInfoAccessor.getAllUsers(); TODO
            IList<User> users = new List<User>();

            //Generate all payments that are due this month
            List<Transaction> transactions = users.Where(user => user.PaymentPlan == PaymentPlan.MONTHLY
                    || (user.PaymentPlan == PaymentPlan.SEMESTERLY && (today.Month == SEMI_DUE_MONTH_1 || today.Month == SEMI_DUE_MONTH_2))
                    || (user.PaymentPlan == PaymentPlan.YEARLY && today.Month == YEAR_DUE_MONTH))
                    .Select(user => new Transaction
                    {
                        UserID = user.UserID,
                        AmountCharged = GenerateAmountDue(user),
                        DateDue = new DateTime(today.Year, today.Month, DUE_DAY),
                        ProcessState = ProcessState.NOT_YET_CHARGED
                    }).ToList();

            //Store them in the database
            transactions.ForEach(t => setTransactionAccessor.AddTransaction(t));

            return transactions;
        }

        //Helper method to generate the total amount due for a user's payment
        private double GenerateAmountDue(User user)
        {
            double amountDue = user.Students.Select(s => s.Grade).Aggregate(0, (total, grade) =>
            {
                if (grade < 0 || grade > 12)
                {
                    throw new ArgumentOutOfRangeException("Grade out of bounds: " + grade);
                }
                else if (grade <= 6)
                {
                    total += TUITION_K_6;
                }
                else if (grade <= 8)
                {
                    total += TUITION_7_8;
                }
                else
                {
                    total += TUITION_9_12;
                }
                return total;
            });

            if (user.PaymentPlan == PaymentPlan.MONTHLY)
            {
                amountDue /= 12;
            }
            else if (user.PaymentPlan == PaymentPlan.SEMESTERLY)
            {
                amountDue /= 2;
            }

            return amountDue;
        }
    }
}
