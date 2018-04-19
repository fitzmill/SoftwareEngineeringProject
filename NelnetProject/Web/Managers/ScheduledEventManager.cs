using Core;
using Core.Interfaces;
using Core.Interfaces.Engines;
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

        private readonly double _timerInterval;
        private readonly int _chargingHour;
        private readonly int _reportGenerationHour;

        private readonly IGetTransactionEngine _getTransactionEngine;
        private readonly IPaymentEngine _paymentEngine;
        private readonly INotificationEngine _notificationEngine;
        private readonly ISetReportEngine _setReportEngine;
        private readonly IUserEngine _userEngine;

        public ScheduledEventManager(double timerInterval, int chargingHour, int reportGenerationHour, 
            IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine, 
            INotificationEngine notificationEngine, ISetReportEngine setReportEngine,
            IUserEngine userEngine)
        {
            _timerInterval = timerInterval;
            _chargingHour = chargingHour;
            _reportGenerationHour = reportGenerationHour;
            
            _getTransactionEngine = getTransactionEngine;
            _paymentEngine = paymentEngine;
            _notificationEngine = notificationEngine;
            _setReportEngine = setReportEngine;

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
            if (now.Hour == _chargingHour && now.Day == 1)
            {
                var users = _userEngine.GetAllUsers();
                IEnumerable<Transaction> generatedTransactions = _paymentEngine.GeneratePayments(users, now);
                _notificationEngine.SendTransactionNotifications(generatedTransactions.ToList());
            }
            else if (now.Hour == _chargingHour && now.Day >= TuitionUtil.DUE_DAY && now.Day <= TuitionUtil.DUE_DAY + TuitionUtil.OVERDUE_RETRY_PERIOD)
            {
                IList<Transaction> unsettledTransactions = _getTransactionEngine.GetAllUnsettledTransactions();
                IEnumerable<Transaction> transactionResults = _paymentEngine.ChargePayments(unsettledTransactions.ToList(), now);
                _notificationEngine.SendTransactionNotifications(transactionResults.ToList());
            }

            //Generating Monthly Reports
            if (now.Day == 1 && now.Hour == _reportGenerationHour) {
                DateTime today = new DateTime(now.Year, now.Month, 1);
                _setReportEngine.InsertReport(today.AddMonths(-1), today.AddDays(-1));
            }
        }
 
    }
}