using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines.Utils
{
    /// <summary>
    /// Utility class for calculating tuition and overdue status.
    /// </summary>
    public class TuitionUtil
    {
        public static readonly int OVERDUE_RETRY_PERIOD = 7;
        public static readonly int DUE_DAY = 5;
        public static readonly double PROCESSING_FEE = 1.03;
        public static readonly double LATE_FEE = 25 * PROCESSING_FEE;
        public static readonly int DEFAULT_PRECISION = 2;

        private static Dictionary<int, int> rates = new Dictionary<int, int>()
        {
            {0, 2500},
            {1, 2500},
            {2, 2500},
            {3, 2500},
            {4, 2500},
            {5, 2500},
            {6, 2500},
            {7, 3750},
            {8, 3750},
            {9, 5000},
            {10, 5000},
            {11, 5000},
            {12, 5000}
        };

        private static Dictionary<PaymentPlan, List<int>> monthsDue = new Dictionary<PaymentPlan, List<int>>()
        {
            { PaymentPlan.MONTHLY, new List<int>() { 1, 2, 3, 4, 5, 8, 9, 10, 11, 12 } },
            { PaymentPlan.SEMESTERLY, new List<int>() { 2, 9 } },
            { PaymentPlan.YEARLY, new List<int>() { 9 } }
        };

        /// <summary>
        /// Calculate if payment is due for the current month, given a payment plan
        /// </summary>
        /// <param name="plan">The payment plan of the user</param>
        /// <param name="today">The date for today</param>
        /// <returns>If a payment is due</returns>
        public static bool IsPaymentDue(PaymentPlan plan, DateTime today = default(DateTime))
        {
            if (today == default(DateTime))
            {
                today = DateTime.Now;
            }

            return monthsDue[plan].Contains(today.Month);
        }

        /// <summary>
        /// Compute the date of the next payment for the given plan
        /// </summary>
        /// <param name="plan">The payment plan of the user</param>
        /// <param name="today">The date for today</param>
        /// <returns>When a new payment is due</returns>
        public static DateTime NextPaymentDueDate(PaymentPlan plan, DateTime today = default(DateTime))
        {
            if (today == default(DateTime))
            {
                today = DateTime.Now;
            }

            int monthIndex = monthsDue[plan].FindIndex(m => m >= today.Month);

            //If after last pay period of the year, it's due next year.
            if (monthIndex == -1)
            {
                monthIndex = 0;
            }
            //If during a pay month, but after the due date, it's due next period.
            else if (today.Month == monthsDue[plan][monthIndex] && today.Day > DUE_DAY)
            {
                monthIndex++;
                //If incrementing the period puts it after the end of the year, move the index.
                if (monthIndex >= monthsDue[plan].Count)
                {
                    monthIndex = 0;
                }
            }
            int month = monthsDue[plan][monthIndex];
            int year = today.Year;

            //If you're on the yearly period and already paid for the year
            bool isAfterPeriodForYearly = (plan == PaymentPlan.YEARLY && new DateTime(today.Year, month, DUE_DAY) < today);
            
            //If pay period is next year
            if (month < today.Month || isAfterPeriodForYearly)
            {
                year++;
            }

            return new DateTime(year, month, DUE_DAY);
        }

        /// <summary>
        /// Generate the aggregate amount due for the month by summing the yearly cost for each of
        /// the user's students and dividing by the number of pay periods in the payment plan
        /// </summary>
        /// <param name="user">The user for the transaction</param>
        /// <param name="precision">The precision wanted</param>
        /// <param name="lastTransactionAmountDue">The last transaction amount due</param>
        /// <returns>The amount due</returns>
        public static double GenerateAmountDue(User user, int precision, double lastTransactionAmountDue = 0.0)
        {
            double yearlyAmount = 0;
            foreach (int grade in user.Students.Select(s => s.Grade)) {
                yearlyAmount += rates[grade];
            }

            double periodAmount = (yearlyAmount / monthsDue[user.Plan].Count()) * PROCESSING_FEE;
            double amountDue = Math.Round(periodAmount, precision);

            //add on late fee if the last transaction
            if (lastTransactionAmountDue != 0)
            {
                amountDue = amountDue + lastTransactionAmountDue + LATE_FEE;
            }
            return amountDue;
        }

        /// <summary>
        /// Returns the number of days the transcation is overdue
        /// </summary>
        /// <param name="t">the given transaction</param>
        /// <param name="today">The date for today</param>
        /// <returns>How many days the transaction is overdue</returns>
        public static int DaysOverdue(Transaction t, DateTime today = default(DateTime))
        {
            if (today == default(DateTime))
            {
                today = DateTime.Now;
            }

            return today.Subtract(t.DateDue).Days;
        }

        /// <summary>
        /// Returns if number of days overdue is greater or equal to grace period
        /// </summary>
        /// <param name="t">the given transaction</param>
        /// <param name="today">The date for today</param>
        /// <returns>If it is past the retry period</returns>
        public static bool IsPastRetryPeriod(Transaction t, DateTime today = default(DateTime))
        {
            if (today == default(DateTime))
            {
                today = DateTime.Now;
            }

            return DaysOverdue(t, today) >= OVERDUE_RETRY_PERIOD;
        }
    }
}
