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
    public class TutitionUtil
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

        /// <summary>
        /// Get the yearly tuition rate for a given grade.
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static int TuitionForGrade(int grade)
        {
            return rates[grade];
        }

    }
}
