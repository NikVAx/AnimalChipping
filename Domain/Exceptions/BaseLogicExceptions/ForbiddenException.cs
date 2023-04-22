using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() { }

        public ForbiddenException(string? message) : base(message) { }

        public ForbiddenException(string? message, Exception? innerException) : base(message, innerException) { }

        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
