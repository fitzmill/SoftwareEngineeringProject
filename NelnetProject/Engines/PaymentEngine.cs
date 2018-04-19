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
        private IGetUserInfoAccessor getUserInfoAccessor;
        private IGetPaymentInfoAccessor getPaymentInfoAccessor;
        private IChargePaymentAccessor chargePaymentAccessor;
        private ISetTransactionAccessor setTransactionAccessor;
        private IGetTransactionAccessor getTransactionAccessor;

        public PaymentEngine(IGetUserInfoAccessor getUserInfoAccessor, IGetPaymentInfoAccessor getPaymentInfoAccessor, 
            IChargePaymentAccessor chargePaymentAccessor, ISetTransactionAccessor setTransactionAccessor, IGetTransactionAccessor getTransactionAccessor)
        {
            this.getUserInfoAccessor = getUserInfoAccessor;
            this.getPaymentInfoAccessor = getPaymentInfoAccessor;
            this.chargePaymentAccessor = chargePaymentAccessor;
            this.setTransactionAccessor = setTransactionAccessor;
            this.getTransactionAccessor = getTransactionAccessor;
        }

        public IList<Transaction> ChargePayments(List<Transaction> charges, DateTime today)
        {
            IList<Transaction> result = new List<Transaction>();
            foreach (Transaction charge in charges)
            {
                result.Add(ChargePayment(charge, today));
            }
            return result;
        }

        private Transaction ChargePayment(Transaction charge, DateTime today)
        {
            //Create dto to be sent to Payment Spring
            PaymentDTO payment = new PaymentDTO
            {
                CustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(charge.UserID),
                Amount = (int) charge.AmountCharged * 100 //Charge is in cents
            };

            //Charge Payment Spring
            ChargeResultDTO result = chargePaymentAccessor.ChargeCustomer(payment);

            //If charge fails, retry for seven days. After that, the charge is marked as failed.
            ProcessState processState = ProcessState.SUCCESSFUL;
            if (!result.WasSuccessful)
            {
                processState = TuitionUtil.IsPastRetryPeriod(charge, today) ? ProcessState.FAILED : ProcessState.RETRYING;
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
        }

        public IList<Transaction> GeneratePayments(DateTime today) //to be run on the 1st of each month
        {
            //Get all users
            IList<User> users = getUserInfoAccessor.GetAllActiveUsers();

            IList<Transaction> failedTransactions = getTransactionAccessor.GetAllFailedTransactions();

            //Generate all payments that are due this month
            List<Transaction> newTransactions = new List<Transaction>();

            //List all failed transactions that have been deferred to this month
            List<Transaction> transactionsToUpdate = new List<Transaction>();

            IEnumerable<User> usersToCharge = users.Where(user => TuitionUtil.IsPaymentDue(user.Plan, today));

            foreach (User user in usersToCharge)
            {
                //if it doesn't exist, mostRecentTransaction will be null
                Transaction mostRecentTransaction = failedTransactions.FirstOrDefault(t => t.UserID == user.UserID);

                double amountDue = 0;
                if (mostRecentTransaction != null)
                {
                    amountDue = TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION, mostRecentTransaction.AmountCharged);
                    mostRecentTransaction.ProcessState = ProcessState.DEFERRED;
                    transactionsToUpdate.Add(mostRecentTransaction);
                }
                else
                {
                    amountDue = TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION);
                }

                newTransactions.Add(new Transaction()
                {
                    UserID = user.UserID,
                    AmountCharged = amountDue,
                    DateDue = new DateTime(today.Year, today.Month, TuitionUtil.DUE_DAY),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                });
            }

            //Store them in the database
            newTransactions.ForEach(t => setTransactionAccessor.AddTransaction(t));

            //update newly deferred transactionss
            transactionsToUpdate.ForEach(t => setTransactionAccessor.UpdateTransaction(t));

            return newTransactions;
        }

        public Transaction CalculateNextPaymentForUser(int userID, DateTime today)
        {
            List<Transaction> userTransactions = getTransactionAccessor.GetAllFailedTransactions().ToList();
            Transaction failed = userTransactions.Find(t => t.UserID == userID);
            double overdueAmount = (failed == null) ? 0.0 : failed.AmountCharged;

            User user = getUserInfoAccessor.GetUserInfoByID(userID);
            double nextAmount = TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION, overdueAmount);
            DateTime dueDate = TuitionUtil.NextPaymentDueDate(user.Plan, today);

            return new Transaction()
            {
                UserID = userID,
                AmountCharged = nextAmount,
                DateDue = dueDate,
                ProcessState = ProcessState.NOT_YET_CHARGED
            };
        }

        public double CalculatePeriodicPayment(User user)
        {
            return TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION);
        }
    }
}
