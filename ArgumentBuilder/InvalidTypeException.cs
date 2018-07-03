using System;
using System.Runtime.Serialization;

namespace ArgumentBuilder
{
    /// <summary>
    /// Thrown when an invalid or unexpected type is used.
    /// </summary>
    class InvalidTypeException : Exception
    {
        public InvalidTypeException()
        {
        }

        public InvalidTypeException(string message) : base(message)
        {
        }

        public InvalidTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
