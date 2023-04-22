using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    /// <summary>
    /// An exception that occurs when business operations cannot be performed.
    /// </summary>
    public class InvalidDomainOperationException : Exception
    {
        public InvalidDomainOperationException()
        {
        }

        public InvalidDomainOperationException(string? message) : base(message)
        {
        }

        public InvalidDomainOperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDomainOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
