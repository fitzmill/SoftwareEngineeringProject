using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    /// <summary>
    /// Exception to be used when there are errors with emails
    /// </summary>
    public class EmailException : Exception
    {
        public EmailException()
        {

        }

        public EmailException(string message) : base(message)
        {

        }

        public EmailException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
