using System;
using System.Runtime.Serialization;

namespace makelunch.domain.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
        {
        }

        public TooManyRequestsException(string message) : base(message)
        {
        }

        public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}