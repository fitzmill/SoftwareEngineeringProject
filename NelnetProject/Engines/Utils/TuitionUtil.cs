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
        public static readonly double LATE_FEE = 25 * 1.03;
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

        //Calculate if payment is due for the current month, given a payment plan.
        public static bool IsPaymentDue(PaymentPlan plan, DateTime today)
        {
            return monthsDue[plan].Contains(today.Month);
        }

        //Compute the date of the next payment for the given plan.
        public static DateTime NextPaymentDueDate(PaymentPlan plan, DateTime today)
        {
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
            bool isAfterPeriodForYearly = (plan == PaymentPlan.YEARLY && 
                (today.Month > month || (today.Month == month && today.Day > DUE_DAY)));
            
            //If pay period is next year
            if (month < today.Month || isAfterPeriodForYearly)
            {
                year++;
            }

            return new DateTime(year, month, DUE_DAY);
        }

        //Generate the aggregate amount due for the month by summing the yearly cost for each of
        //the user's students and dividing by the number of pay periods in the payment plan.
        public static double GenerateAmountDue(User user, int precision, double lastTransactionAmountDue = 0.0)
        {
            double yearlyAmount = 0;
            foreach (int grade in user.Students.Select(s => s.Grade)) {
                yearlyAmount += rates[grade];
            }

            double periodAmount = yearlyAmount / monthsDue[user.Plan].Count();
            double amountDue = Math.Round(periodAmount, precision);

            //add on late fee if the last transaction
            amountDue = lastTransactionAmountDue == 0 ? amountDue : amountDue + lastTransactionAmountDue + LATE_FEE;
            return amountDue;
        }

        //Returns the number of days the transaction is overdue
        public static int DaysOverdue(Transaction t, DateTime today)
        {
            return today.Subtract(t.DateDue).Days;
        }

        //Returns if number of days overdue is greater or equal to grace period
        public static bool IsPastRetryPeriod(Transaction t, DateTime today)
        {
            return DaysOverdue(t, today) >= OVERDUE_RETRY_PERIOD;
        }
    }
}
