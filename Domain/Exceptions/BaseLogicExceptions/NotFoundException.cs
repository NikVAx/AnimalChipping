using System.Runtime.Serialization;

namespace Domain.Exceptions.BaseLogicExceptions
{
    /// <summary>
    /// An exception that occurs when entity not found in source object.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string? message) : base(message) { }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        
        public NotFoundException(Type entityType, object key, Exception? innerException = null) :
            base($"'{entityType.Name}' with key '{key}' not found", innerException) { }

        public NotFoundException(Type entityType, object key,
            Type sourceType, object sourceKey, Exception? innerException = null) :
            base($"'{entityType.Name}' with key '{key}' not found in '{sourceType.Name} with key '{sourceKey}'",
                innerException) { }
        
    }
}
