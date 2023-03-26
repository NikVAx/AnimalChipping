using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class OperationException : Exception
    {
        public OperationException()
        {
        }

        public OperationException(string? message) : base(message)
        {
        }

        public OperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
