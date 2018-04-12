using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class SqlRowNotAffectedException : DbException
    {
        public SqlRowNotAffectedException(string message) : base(message)
        {
        }

        public SqlRowNotAffectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SqlRowNotAffectedException(string message, int errorCode) : base(message, errorCode)
        {
        }

        public SqlRowNotAffectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
