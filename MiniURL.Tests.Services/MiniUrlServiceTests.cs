using Microsoft.EntityFrameworkCore;
using MiniURL.Core;
using MiniURL.Core.Models;
using MiniURL.Services.Implementations;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using MiniURL.Services.Exceptions;

namespace MiniURL.Tests.Services
{
    [TestFixture]
    public class MiniUrlServiceTests
    {
        MiniURLContext db;
        string validRef;
        [SetUp]
        public async Task Setup()
        {
            db = new MiniURLContext(new DbContextOptionsBuilder<MiniURLContext>().UseInMemoryDatabase("TestDB").Options);
            var user = new User("Mr", "Test", "Account", "daniel.layland@test.com", new UserLevel("Guest", "some description", 1));
            user.CreateNewMiniUrl("https://www.google.com");
            db.Users.Add(user);
            await db.SaveChangesAsync();
            validRef = (await db.MiniUrls.FirstOrDefaultAsync())?.Reference;
        }

        [TearDown]
        public void TearDown()
        {
            validRef = null;
            db.MiniUrls.RemoveRange(db.MiniUrls.ToList());
            db.Users.RemoveRange(db.Users.ToList());
            db.UserLevels.RemoveRange(db.UserLevels.ToList());
            db.SaveChanges();
            db.Dispose();
        }

        [Test]
        public async Task Given_IHaveAMiniURLRecord_AndItHasntExpired_WhenITryToGetIt_ThenTheResultIsReturned()
        {
            var SUT = new MiniUrlService(db);
            var res = await SUT.GetByRef(validRef);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.Reference, Is.EqualTo(validRef));
        }

        [Test]
        public async Task Given_IHaveAMiniURLRecord_AndItHasntExpired_WhenITryToGetTheUrl_ThenTheResultIsReturned()
        {
            var SUT = new MiniUrlService(db);
            var res = await SUT.GetUrlByRef(validRef);

            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.EqualTo("https://www.google.com/"));
        }

        [Test]
        public async Task Given_IHaveAMiniURLRecord_AndItHasExpired_WhenITryToGetIt_ThenANotFoundExceptionIsThrown()
        {
            var urls = await db.MiniUrls.ToListAsync();
            foreach (var url in urls)
                url.UpdateExpiry(-10);
            await db.SaveChangesAsync();
            var SUT = new MiniUrlService(db);

            Assert.ThrowsAsync<NotFoundException>(() => SUT.GetByRef(validRef));
        }
    }
}