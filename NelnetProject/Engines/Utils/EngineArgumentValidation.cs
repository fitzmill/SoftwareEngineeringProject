using System;
using System.Diagnostics;

namespace Engines.Utils
{
    /// <summary>
    /// Utility class for validating method arguments at the engine level.
    /// </summary>
    public static class EngineArgumentValidation
    {
        /// <summary>
        /// Verifies that the argument is not null.
        /// </summary>
        /// <param name="value">the arg to test</param>
        /// <param name="argument">the name of the argument</param>
        [DebuggerHidden]
        public static void ArgumentIsNotNull(object value, string argument)
        {
            if (value == null)
                throw new ArgumentNullException($"{argument} cannot be null");
        }

        /// <summary>
        /// Verifies that the argument is not negative.
        /// </summary>
        /// <param name="value">the arg to test</param>
        /// <param name="argument">the name of the argument</param>
        [DebuggerHidden]
        public static void ArgumentIsNonNegative(double value, string argument)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"{argument} cannot be negative");
            }
        }

        /// <summary>
        /// Verifies that the argument is not negative.
        /// </summary>
        /// <param name="value">the arg to test</param>
        /// <param name="argument">the name of the argument</param>
        [DebuggerHidden]
        public static void ArgumentIsNonNegative(int value, string argument)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"{argument} cannot be negative");
            }
        }

        /// <summary>
        /// Verifies that the string is not empty.
        /// </summary>
        /// <param name="value">the string to test</param>
        /// <param name="argument">the name of the argument</param>
        [DebuggerHidden]
        public static void StringIsNotEmpty(string value, string argument)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"{argument} cannot be empty or null");
            }
        }
    }
}
