using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace Web.Managers
{
    public class PaymentManager
    {
        static double timerInterval = 1000; //todo move

        IGetTransactionEngine getTransactionEngine;
        IPaymentEngine paymentEngine;
        INotificationEngine notificationEngine;

        public PaymentManager(IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine, INotificationEngine notificationEngine)
        {
            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
            this.notificationEngine = notificationEngine;

            Timer timer = new Timer(timerInterval);
            timer.Elapsed += new ElapsedEventHandler(TimerIntervalElapsed);
            timer.Enabled = true;
        }

        public void TimerIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("time elapsed");
        }
    }
}