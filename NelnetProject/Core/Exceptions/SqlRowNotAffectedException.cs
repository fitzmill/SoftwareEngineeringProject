using System;
using System.Data.Common;
using System.Runtime.Serialization;

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
