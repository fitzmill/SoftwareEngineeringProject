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
    class PaymentEngine : IPaymentEngine
    {
        private IGetUserInfoAccessor getUserInfoAccessor;
        private IGetPaymentInfoAccessor getPaymentInfoAccessor;
        private IChargePaymentAccessor chargePaymentAccessor;

        public PaymentEngine(IGetUserInfoAccessor getUserInfoAccessor, IGetPaymentInfoAccessor getPaymentInfoAccessor, IChargePaymentAccessor chargePaymentAccessor)
        {
            this.getUserInfoAccessor = getUserInfoAccessor;
            this.getPaymentInfoAccessor = getPaymentInfoAccessor;
            this.chargePaymentAccessor = chargePaymentAccessor;
        }

        public IList<Transaction> ChargePayments(List<Transaction> charges)
        {
           return charges.Select(charge =>
           {
               PaymentDTO payment = new PaymentDTO
               {
                   CustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(charge.UserID),
                   Amount = charge.AmountCharged
               };

               ChargeResultDTO result = chargePaymentAccessor.ChargeCustomer(payment);

               ProcessState processState = ProcessState.SUCCESSFUL;
               if (!result.WasSuccessful)
               {
                   int daysOverdue = DateTime.Today.Subtract(charge.DateDue).Days;
                   processState = (daysOverdue >= 7) ? ProcessState.FAILED : ProcessState.RETRYING;
               }

               return new Transaction
               {
                   TransactionID = charge.TransactionID,
                   UserID = charge.UserID,
                   AmountCharged = charge.AmountCharged,
                   DateDue = charge.DateDue,
                   DateCharged = DateTime.Today,
                   ProcessState = processState,
                   ReasonFailed = result.ErrorMessage
               };
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
