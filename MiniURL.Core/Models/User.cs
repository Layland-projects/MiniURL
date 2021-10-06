using MiniURL.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Models
{
    public class User : EntityBase
    {
        public string Title { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public ICollection<MiniURL> MiniUrls { get; private set; } = new List<MiniURL>();
        public UserLevel Level { get; private set; }
        public Guid LevelId { get; private set; }
        internal User()
        {

        }

        public User(string title, string firstName, string lastName, string email, UserLevel level)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            if (email is not null)
                Email = new Email(email);
            //in production I would also check for levels with default Id's
            //so that the level must already exist to be used here, but for an easier demo I won't add that now
            Level = level ?? throw new ArgumentNullException(nameof(level));
            LevelId = level.Id;
        }

        public void UpdateEmail(string email) => Email = new Email(email);

        public void CreateNewMiniUrl(Url url)
        {
            MiniUrls.Add(new MiniURL(this, url));
        }

        public void CreateNewMiniUrl(string url)
        {
            CreateNewMiniUrl(new Url(url));
        }

        public void UpdateLevel(UserLevel level)
        {
            Level = level ?? throw new ArgumentNullException(nameof(level));
            LevelId = level.Id;
            foreach (var miniUrl in MiniUrls)
                miniUrl.UpdateExpiry(level.LinkDurationDays); 
        }
    }
}
