using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ReportException : Exception
    {
        public ReportException() : base()
        {

        }

        public ReportException(string message) : base(message)
        {

        }

        public ReportException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
