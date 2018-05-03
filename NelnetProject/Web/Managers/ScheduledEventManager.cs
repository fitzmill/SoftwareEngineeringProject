using Core;
using Core.Interfaces.Engines;
using Engines.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Web.Managers
{
    /// <summary>
    /// Manager for executing events at scheduled intervals.
    /// </summary>
    public class ScheduledEventManager
    {
        private readonly double _timerInterval;
        private readonly int _chargingHour;
        private readonly int _reportGenerationHour;

        private readonly ITransactionEngine _transactionEngine;
        private readonly IPaymentEngine _paymentEngine;
        private readonly INotificationEngine _notificationEngine;
        private readonly IReportEngine _reportEngine;
        private readonly IUserEngine _userEngine;

        public ScheduledEventManager(
            double timerInterval, 
            int chargingHour, 
            int reportGenerationHour, 
            ITransactionEngine transactionEngine, 
            IPaymentEngine paymentEngine, 
            INotificationEngine notificationEngine, 
            IReportEngine reportEngine,
            IUserEngine userEngine)
        {
            _timerInterval = timerInterval;
            _chargingHour = chargingHour;
            _reportGenerationHour = reportGenerationHour;

            _transactionEngine = transactionEngine;
            _paymentEngine = paymentEngine;
            _notificationEngine = notificationEngine;
            _reportEngine = reportEngine;
            _userEngine = userEngine;

            Timer timer = new Timer(timerInterval);
            timer.Elapsed += new ElapsedEventHandler(TimerIntervalElapsed);
            timer.Enabled = true;

            TimerIntervalElapsed(null, null); //Initiate the event once on startup
        }

        /// <summary>
        /// Executed every _timerInterval.
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Any event arguments</param>
        public void TimerIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
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
                IEnumerable<Transaction> unsettledTransactions = _transactionEngine.GetAllUnsettledTransactions();
                IEnumerable<Transaction> transactionResults = _paymentEngine.ChargePayments(unsettledTransactions.ToList(), now);
                _notificationEngine.SendTransactionNotifications(transactionResults.ToList());
            }

            //Generating Monthly Reports
            if (now.Day == 1 && now.Hour == _reportGenerationHour) {
                DateTime today = new DateTime(now.Year, now.Month, 1);
                _reportEngine.InsertReport(today.AddMonths(-1), today.AddDays(-1));
            }
        }
 
    }
}