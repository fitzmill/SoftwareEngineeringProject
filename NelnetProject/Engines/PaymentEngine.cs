using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.DTOs;
using Core.Interfaces;

namespace Engines
{
    /// <summary>
    /// Charges payments to users.
    /// </summary>
    public class PaymentEngine : IPaymentEngine
    {
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
                   processState = (daysOverdue >= 7) ? ProcessState.FAILED : ProcessState.RETRYING;
               }

               //Generate result transaction
               Transaction resultTransaction =  new Transaction
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

        public IList<Transaction> GeneratePayments()
        {
            //Generates payments
            //Stores them in the database
            throw new NotImplementedException();
        }
    }
}
