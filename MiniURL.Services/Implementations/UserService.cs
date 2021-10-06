using Microsoft.EntityFrameworkCore;
using MiniURL.Core;
using MiniURL.Core.Models;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Interfaces;
using MiniURL.Services.Models.Request;
using MiniURL.Services.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Implementations
{
    public class UserService : IUserService, IAsyncDisposable, IDisposable
    {
        private readonly MiniURLContext db;
        public UserService(MiniURLContext db)
        {
            this.db = db;
        }
        public async Task<Guid> AddUser(User_Create request)
        {
            string fName, lName, title, email;
            fName = lName = title = email = null;
            var level = await db.UserLevels.FindAsync(request.LevelId);
            if (level == null)
                throw new NotFoundException(typeof(UserLevel), new { request.LevelId });
            if (level.Name != "Guest")
            {
                fName = request.FirstName;
                lName = request.LastName;
                title = request.Title;
                email = request.Email;
            }
            var u = new User(title, fName, lName, email, level);
            db.Add(u);
            await db.SaveChangesAsync();
            return u.Id;
        }

        public async Task<string> AddMiniURLRecord(MiniURL_Create request, Guid? userId = null)
        {
            if (!userId.HasValue)
            {
                var guestLevel = await db.UserLevels.Where(x => x.Name == "Guest").FirstOrDefaultAsync();
                userId = await AddUser(new User_Create { LevelId = guestLevel.Id });
            }
            var u = await db.Users.Include(x => x.MiniUrls)
                .Include(x => x.Level)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (u == null)
                throw new NotFoundException(typeof(User), new { UserId = userId });
            u.CreateNewMiniUrl(request.URL);
            var latestUrl = u.MiniUrls.Last();
            while (await db.MiniUrls.Where(x => x.Reference == latestUrl.Reference).AnyAsync())
                latestUrl.GenerateNewReference();
            await db.SaveChangesAsync();
            return u.MiniUrls.Last().Reference;
        }


        public async Task<User_Lite> Get_Lite(Guid Id)
        {
            var u =  await db.Users.Include(x => x.Level)
                .Select(Get_Lite_Selector())
                .Where(x => x.Id == Id)
            .FirstOrDefaultAsync();
            if (u == null)
                throw new NotFoundException(typeof(User), new { Id });
            return u;
        }

        public async IAsyncEnumerable<User_Lite> GetAll_Lite()
        {
            await foreach(var user in db.Users.Include(x => x.Level).Select(Get_Lite_Selector()).AsAsyncEnumerable())
            {
                yield return user;
            }
        }

        public async Task UpdateUserLevel(User_UpdateLevel request)
        {
            var u = await db.Users.Include(x => x.MiniUrls)
                .Include(x => x.Level)
                .Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            if (u == null)
                throw new NotFoundException(typeof(User), new { request.UserId });
            if (u.Level.Id == request.LevelId)
                return;
            var level = await db.UserLevels
                .Where(x => x.Id == request.LevelId)
                .FirstOrDefaultAsync();
            if (level == null)
                throw new NotFoundException(typeof(UserLevel), new { request.LevelId });
            u.UpdateLevel(level);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await db.DisposeAsync();
        }

        private Expression<Func<User, User_Lite>> Get_Lite_Selector() => x => new User_Lite
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt.UtcDateTime,
            UpdatedAt = x.UpdatedAt.HasValue ? x.UpdatedAt.Value.UtcDateTime : null,
            FirstName = x.FirstName,
            Title = x.Title,
            LastName = x.LastName,
            Level = x.Level.Name
        };
    }
}
