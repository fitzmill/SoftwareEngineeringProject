using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException() : base()
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
