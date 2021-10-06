using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Exceptions
{
    public class InvalidUrlException : Exception
    {
        public InvalidUrlException(string message) : base(message)
        {
        }

        public InvalidUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
