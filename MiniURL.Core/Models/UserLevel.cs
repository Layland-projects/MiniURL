using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Models
{
    public class UserLevel : EntityBase
    {
        public string Name { get; private set; } = "";
        public string Description { get; private set; } = "";
        public int? LinkDurationDays { get; private set; } = 0;
        public ICollection<User> Users { get; private set; } = new List<User>();

        internal UserLevel() { }
        public UserLevel(string name, string description, int? duration)
        {
            Name = name;
            Description = description;
            LinkDurationDays = duration;
        }

        internal static UserLevel Seed(string name, string description, int? duration)
        {
            return new()
            {
                Name = name,
                Description = description,
                LinkDurationDays = duration,
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
