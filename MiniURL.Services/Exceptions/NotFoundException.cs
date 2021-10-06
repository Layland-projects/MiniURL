using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type objType, object props = null) : base(FormatMessage(objType, props))
        {
        }

        public NotFoundException(Type objType, Exception innerException, object props = null) : base(FormatMessage(objType, props), innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string FormatMessage(Type objType, object props)
        {
            var sb = new StringBuilder();
            sb.Append($"Unable to find {objType.Name}");
            if (props != null)
            {
                sb.Append(" Values: ");
                foreach (var prop in props.GetType().GetProperties())
                    sb.Append($"\n{prop.Name}: {prop.GetValue(props)}");
            }
            return sb.ToString();
        }
    }
}
