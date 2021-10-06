using MiniURL.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniURL.Core.ValueObjects
{
    public record Email
    {
        public string Username { get; private set; }
        public string Domain { get; private set; }
        internal Email() { }
        public Email(string address)
        {
            if (!IsValidEmail(address))
                throw new InvalidEmailException(address);
            var bits = address.Split('@');
            Username = bits[0];
            Domain = bits[1];
        }
        private bool IsValidEmail(string address)
        {
            try
            {
                var x = new MailAddress(address);
                if (x is not null)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Username}@{Domain}";
        }

        public static implicit operator string(Email e) => e.ToString();
        public static implicit operator Email(string s) => new(s);
    }
}
