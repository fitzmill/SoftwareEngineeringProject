using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Engines.Utils;

namespace Engines
{
    public class PaymentEngine : IPaymentEngine
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IStudentAccessor _studentAccessor;
        private readonly IPaymentAccessor _paymentAccessor;
        private readonly ITransactionAccessor _transactionAccessor;

        public PaymentEngine(IUserAccessor userAccessor, 
            IStudentAccessor studentAccessor,
            IPaymentAccessor paymentAccessor,
            ITransactionAccessor transactionAccessor)
        {
            _userAccessor = userAccessor;
            _studentAccessor = studentAccessor;
            _paymentAccessor = paymentAccessor;
            _transactionAccessor = transactionAccessor;
        }

        public IEnumerable<Transaction> ChargePayments(IEnumerable<Transaction> charges, DateTime today)
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
            EngineArgumentValidation.ArgumentIsNotNull(charge, "charge");
            //Create dto to be sent to Payment Spring
            PaymentDTO payment = new PaymentDTO
            {
                CustomerID = _userAccessor.GetPaymentSpringCustomerID(charge.UserID),
                Amount = (int) charge.AmountCharged * 100 //Charge is in cents
            };

            //Charge Payment Spring
            ChargeResultDTO result = _paymentAccessor.ChargeCustomer(payment);

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
            _transactionAccessor.UpdateTransaction(resultTransaction);

            return resultTransaction;
        }

        public IEnumerable<Transaction> GeneratePayments(IEnumerable<User> users, DateTime today) //to be run on the 1st of each month
        {
            EngineArgumentValidation.ArgumentIsNotNull(users, "users");

            IEnumerable<Transaction> failedTransactions = _transactionAccessor.GetAllFailedTransactions();

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
            newTransactions.ForEach(t => _transactionAccessor.AddTransaction(t));

            //update newly deferred transactionss
            transactionsToUpdate.ForEach(t => _transactionAccessor.UpdateTransaction(t));

            return newTransactions;
        }

        public Transaction CalculateNextPaymentForUser(int userID, DateTime today)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(userID, "User Id");

            List<Transaction> userTransactions = _transactionAccessor.GetAllFailedTransactions().ToList();
            Transaction failed = userTransactions.Find(t => t.UserID == userID);
            double overdueAmount = (failed == null) ? 0.0 : failed.AmountCharged;

            User user = _userAccessor.GetUserInfoByID(userID);
            user.Students = _studentAccessor.GetStudentInfoByUserID(userID);
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
            EngineArgumentValidation.ArgumentIsNotNull(user, "user");
            return TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION);
        }

        public string InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            EngineArgumentValidation.ArgumentIsNotNull(userPaymentInfo, "user payment info");
            return  _paymentAccessor.CreateCustomer(userPaymentInfo);
        }

        public void UpdatePaymentBillingInfo(PaymentAddressDTO paymentAddressInfo)
        {
            EngineArgumentValidation.ArgumentIsNotNull(paymentAddressInfo, "payment address info");
            _paymentAccessor.UpdateCustomerBillingInformation(paymentAddressInfo);
        }

        public void UpdatePaymentCardInfo(PaymentCardDTO paymentCardInfo)
        {
            EngineArgumentValidation.ArgumentIsNotNull(paymentCardInfo, "payment card info");
            _paymentAccessor.UpdateCustomerCardInformation(paymentCardInfo);
        }

        public void DeletePaymentInfo(string customerID)
        {
            EngineArgumentValidation.StringIsNotEmpty(customerID, "customer Id");
            _paymentAccessor.DeleteCustomer(customerID);
        }

        public UserPaymentInfoDTO GetPaymentInfoForUser(int userID)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(userID, "user Id");
            string customerID = _userAccessor.GetPaymentSpringCustomerID(userID);
            return _paymentAccessor.GetPaymentInfoForCustomer(customerID);
        }
    }
}
