using MiniURL.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Models
{
    public class MiniURL : EntityBase
    {
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public string Reference { get; private set; }
        public Url Url { get; private set; }
        public DateTimeOffset? ExpiresOn { get; private set; }
        internal MiniURL()
        {
            if (string.IsNullOrEmpty(Reference))
                GenerateNewReference();
        }

        internal MiniURL(User user, Url url) : this()
        {
            if (user.Level == null)
                throw new ArgumentException("User must have level included to create a new MiniURL Entry");
            User = user;
            Url = url;
            UserId = user.Id;
            if (user.Level.LinkDurationDays.HasValue)
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(user.Level.LinkDurationDays.Value);
        }

        public void GenerateNewReference()
        {
            var sb = new StringBuilder();
            var rand = new Random();
            for(int i = 0; i < 7; i++)
            {
                var c1 = (char)rand.Next(48, 57);
                var c2 = (char)rand.Next(97, 122);
                if (rand.Next(0, 2) > 0)
                    sb.Append(c2);
                else
                    sb.Append(c1);
            }
            Reference = sb.ToString();
        }

        public void UpdateExpiry(int? linkDurationDays)
        {
            if (!linkDurationDays.HasValue)
                ExpiresOn = null;
            else if (CreatedAt == DateTimeOffset.MinValue)
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(linkDurationDays.Value);
            else
                ExpiresOn = CreatedAt.AddDays(linkDurationDays.Value);
        }
    }
}
