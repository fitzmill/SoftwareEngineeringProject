using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Web.Managers
{
    public class PaymentManager
    {
        IGetTransactionEngine getTransactionEngine;
        IPaymentEngine paymentEngine;
        INotificationEngine notificationEngine;

        public PaymentManager(IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine, INotificationEngine notificationEngine)
        {
            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
            this.notificationEngine = notificationEngine;
        }
    }
}