using System;
using System.Runtime.Serialization;

namespace GomokuGame
{
    public class GomokuGameException : Exception
    {
        public GomokuGameException()
        {
        }

        public GomokuGameException(string message) : base(message)
        {
        }

        public GomokuGameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GomokuGameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
