using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines.Utils
{
    /// <summary>
    /// Utility class for calculating tuition.
    /// </summary>
    public class TuitionUtil
    {
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
            { PaymentPlan.MONTHLY, new List<int>() { 8, 9, 10, 11, 12, 1, 2, 3, 4, 5 } },
            { PaymentPlan.SEMESTERLY, new List<int>() { 9, 2 } },
            { PaymentPlan.YEARLY, new List<int>() { 9 } }
        };

        //Calculate if payment is due for the current month, given a payment plan.
        public static bool IsPaymentDue(PaymentPlan plan, DateTime today)
        {
            return monthsDue[plan].Contains(today.Month);
        }

        //Generatethe aggregate amount due for the month by summing the yearly cost for each of
        //the user's students and dividing by the number of pay periods in the payment plan.
        public static double GenerateAmountDue(User user, int precision)
        {
            double yearlyAmount = 0;
            foreach (int grade in user.Students.Select(s => s.Grade)) {
                yearlyAmount += rates[grade];
            }

            double periodAmount = yearlyAmount / monthsDue[user.Plan].Count();
            return Math.Round(periodAmount, precision);
        }
    }
}
