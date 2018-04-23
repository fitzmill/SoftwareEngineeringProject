﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines.Utils
{
    public static class EngineArgumentValidation
    {
        [DebuggerHidden]
        public static void ArgumentIsNotNull(object value, string argument)
        {
            if (value == null)
                throw new ArgumentNullException($"{argument} cannot be null");
        }

        [DebuggerHidden]
        public static void ArgumentIsNonNegative(double value, string argument)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"{argument} cannot be negative");
            }
        }

        [DebuggerHidden]
        public static void ArgumentIsNonNegative(int value, string argument)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException($"{argument} cannot be negative");
            }
        }

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
