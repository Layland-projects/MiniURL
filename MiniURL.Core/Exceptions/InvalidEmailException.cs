using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
        {
        }

        public InvalidEmailException(string address) : base(MessageFormatter(address))
        {
        }

        public InvalidEmailException(string address, Exception innerException) : base(MessageFormatter(address), innerException)
        {
        }

        protected InvalidEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string MessageFormatter(string address) => $"Address: {address} is invalid";
    }
}
