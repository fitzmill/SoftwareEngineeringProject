using Core;
using Core.Interfaces;
using Engines.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Web.Managers
{
    public class ScheduledEventManager
    {
        public Func<DateTime> dateProvider = () => DateTime.Now;

        private double timerInterval;
        private int chargingHour;
        private int reportGenerationHour;

        private IGetTransactionEngine getTransactionEngine;
        private IPaymentEngine paymentEngine;
        private INotificationEngine notificationEngine;
        private ISetReportEngine setReportEngine;

        public ScheduledEventManager(double timerInterval, int chargingHour, int reportGenerationHour, 
            IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine, 
            INotificationEngine notificationEngine, ISetReportEngine setReportEngine)
        {
            this.timerInterval = timerInterval;
            this.chargingHour = chargingHour;
            this.reportGenerationHour = reportGenerationHour;

            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
            this.notificationEngine = notificationEngine;
            this.setReportEngine = setReportEngine;

            Timer timer = new Timer(timerInterval);
            timer.Elapsed += new ElapsedEventHandler(TimerIntervalElapsed);
            timer.Enabled = true;

            TimerIntervalElapsed(null, null); //Initiate the event once on startup
        }

        public void TimerIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime now = dateProvider();
            Debug.WriteLine(String.Format("Time Elapsed at {0:yyyy MM dd HH mm ss}", now));
            
            //Generating Payments
            if (now.Hour == chargingHour && now.Day == 1)
            {
                IList<Transaction> generatedTransactions = paymentEngine.GeneratePayments(now);
                notificationEngine.SendTransactionNotifications(generatedTransactions.ToList());
            }
            else if (now.Hour == chargingHour && now.Day >= TuitionUtil.DUE_DAY && now.Day <= TuitionUtil.DUE_DAY + TuitionUtil.OVERDUE_RETRY_PERIOD)
            {
                IList<Transaction> unsettledTransactions = getTransactionEngine.GetAllUnsettledTransactions();
                IList<Transaction> transactionResults = paymentEngine.ChargePayments(unsettledTransactions.ToList(), now);
                notificationEngine.SendTransactionNotifications(transactionResults.ToList());
            }

            //Generating Monthly Reports
            if (now.Day == 1 && now.Hour == reportGenerationHour) {
                DateTime today = new DateTime(now.Year, now.Month, 1);
                setReportEngine.InsertReport(today.AddMonths(-1), today.AddDays(-1));
            }
        }
 
    }
}