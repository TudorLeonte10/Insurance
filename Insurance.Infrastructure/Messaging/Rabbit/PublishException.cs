using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Insurance.Infrastructure.Messaging.Rabbit
{
    [ExcludeFromCodeCoverage]
    public class PublishRQException : Exception
    {
        public PublishRQException() { }
        public PublishRQException(string message) : base(message)
        {
        }

        public PublishRQException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
