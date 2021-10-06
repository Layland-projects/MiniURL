using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniURL.Core;
using MiniURL.Core.Exceptions;
using MiniURL.Core.Models;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Implementations;
using MiniURL.Services.Models.Request;
using NUnit.Framework;

namespace MiniURL.Tests.Services
{
    public class UserServiceTests
    {
        MiniURLContext db;
        [SetUp]
        public async Task Setup()
        {
            db = new MiniURLContext(new DbContextOptionsBuilder<MiniURLContext>().UseInMemoryDatabase("TestDB").Options);
            var user = new User("Mr", "Dan", "Account", "daniel.layland@test.com", new UserLevel("Guest", "some description", 1));
            var user2 = new User("Mrs", "Test", "Account", "danielle.test@e.com", new UserLevel("Regular", "some description", 5));
            db.Users.AddRange(new User[] { user, user2 });
            await db.SaveChangesAsync();
        }
        [TearDown]
        public async Task TearDown()
        {
            db.Users.RemoveRange(db.Users.ToList());
            db.UserLevels.RemoveRange(db.UserLevels.ToList());
            db.MiniUrls.RemoveRange(db.MiniUrls.ToList());
            await db.SaveChangesAsync();
            db.Dispose();
        }
        #region AddUser
        [Test]
        public async Task Given_IHaveAValidUser_CreateRequest_AndTheLevelIsRegular_WhenIAddNewUser_ThenTheUserIsCreatedCorrectly()
        {
            var levelId = (await db.UserLevels.FirstOrDefaultAsync(x => x.Name == "Regular")).Id;
            var command = new User_Create { Email = "dan.text@123.com", Title = "Mr", FirstName = "Dan", LastName = "Test", LevelId = levelId };
            var SUT = new UserService(db);
            var res = await SUT.AddUser(command);
            Assert.That(res, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task Given_IHaveAUser_CreateRequestWithABadEmail_AndTheLevelIsRegular_WhenIAddNewUser_ThenAnInvalidEmailExceptionIsThrown()
        {
            var levelId = (await db.UserLevels.FirstOrDefaultAsync(x => x.Name == "Regular")).Id;
            var command = new User_Create { Email = "help", Title = "Mr", FirstName = "Dan", LastName = "Test", LevelId = levelId };
            var SUT = new UserService(db);
            Assert.ThrowsAsync<InvalidEmailException>(() => SUT.AddUser(command));
        }

        [Test]
        public void Given_IHaveAUser_CreateRequestWithABadLevelId_WhenIAddNewUser_ThenANotFoundExceptionIsThrown()
        {
            var command = new User_Create { Email = "help", Title = "Mr", FirstName = "Dan", LastName = "Test", LevelId = Guid.Empty };
            var SUT = new UserService(db);
            Assert.ThrowsAsync<NotFoundException>(() => SUT.AddUser(command));
        }

        [Test]
        public async Task Given_IHaveAUser_CreateRequestWithAllNulls_AndTheLevelIsGuest_WhenIAddNewUser_ThenTheUserIsCreatedCorrectly()
        {
            var levelId = (await db.UserLevels.FirstOrDefaultAsync(x => x.Name == "Guest")).Id;
            var command = new User_Create { Email = null, Title = null, FirstName = null, LastName = null, LevelId = levelId };
            var SUT = new UserService(db);
            var res = SUT.AddUser(command);
            Assert.That(res, Is.Not.EqualTo(Guid.Empty));
        }
        #endregion
        #region AddMiniURLRecord
        [Test]
        public async Task Given_IHaveAValidMiniURL_CreateRequest_AndAValidUserGuid_WhenIAddMiniURLRecord_ThenTheRecordShouldBeAdded_AndTheReferenceReturned()
        {
            var command = new MiniURL_Create { URL = "https://www.google.com" };
            var userGuid = (await db.Users.FirstOrDefaultAsync())?.Id;
            var SUT = new UserService(db);

            var res = await SUT.AddMiniURLRecord(command, userGuid);
            var url = await db.MiniUrls.Where(x => x.UserId == userGuid).FirstOrDefaultAsync();

            Assert.That(res, Is.Not.Empty);
            Assert.That(res, Is.Not.Null);
            Assert.That(url, Is.Not.Null);
        }

        [Test]
        public async Task Given_IHaveAValidMiniURL_CreateRequest_AndNoGuid_WhenIAddMiniURLRecord_ThenTheRecordShouldBeAdded_AndAssociatedWithANewGuestUser()
        {
            var command = new MiniURL_Create { URL = "https://www.google.com" };
            var SUT = new UserService(db);

            var res = await SUT.AddMiniURLRecord(command);
            var url = await db.MiniUrls.Where(x => x.Reference == res).FirstOrDefaultAsync();
            var newUser = await db.Users.Where(x => x.Id == url.UserId).FirstOrDefaultAsync();

            Assert.That(res, Is.Not.Empty);
            Assert.That(res, Is.Not.Null);
            Assert.That(url, Is.Not.Null);
            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Email, Is.Null);
        }

        [Test]
        public async Task Given_ICreateLotsOfMiniURLs_WhenICheck_TheyAllHaveAUniqueReference() //this test isn't necessarilly 100% accurate, but the best I could think of to validate the feature
        {
            var SUT = new UserService(db);
            for (int i = 0; i < 1000; i++)
            {
                var command = new MiniURL_Create { URL = "https://www.google.com" };
                await SUT.AddMiniURLRecord(command);
            }

            var allRefs = await db.MiniUrls.Select(x => x.Reference).ToListAsync();
            var uniqueRefs = await db.MiniUrls.Select(x => x.Reference).Distinct().ToListAsync();

            Assert.That(allRefs, Has.Count.EqualTo(uniqueRefs.Count));
        }
        #endregion
        #region Get_Lite
        [Test]
        public async Task Given_IHaveAValidUserGuid_WhenIGet_Lite_ThenIShouldReceiveARepresentationOfThatUser()
        {
            var userGuid = (await db.Users.Include(x => x.Level).FirstOrDefaultAsync(x => x.Level.Name == "Guest")).Id;
            var SUT = new UserService(db);
            var res = await SUT.Get_Lite(userGuid);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.Level, Is.EqualTo("Guest"));
            Assert.That(res.FirstName, Is.EqualTo("Dan"));
        }

        [Test]
        public void Given_IHaveAnInvalidUserGuid_WhenIGet_Lite_ThenItShouldThrowANotFoundException()
        {
            var SUT = new UserService(db);
            Assert.ThrowsAsync<NotFoundException>(() => SUT.Get_Lite(Guid.Empty));
        }

        [Test]
        public async Task Given_IHaveUsers_WhenIGetAll_Lite_ThenIShouldHaveMoreThan0Results()
        {
            var SUT = new UserService(db);
            int count = 0;
            await foreach (var x in SUT.GetAll_Lite())
                count++;
            Assert.That(count, Is.GreaterThan(0));
        }
        #endregion
        #region UpdateUserLevel
        [Test]
        public async Task Given_IHaveAValidUser_UpdateLevelRequest_WhenIUpdateUserLevel_ThenTheUsersLevelShouldBeUpdated()
        {
            var levelId = (await db.UserLevels.FirstOrDefaultAsync(x => x.Name == "Regular")).Id;
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.FirstName == "Dan")).Id;
            var command = new User_UpdateLevel { LevelId = levelId, UserId = userId };
            var SUT = new UserService(db);

            await SUT.UpdateUserLevel(command);

            var res = await SUT.Get_Lite(userId);
            Assert.That(res.Level, Is.EqualTo("Regular"));
        }

        [Test]
        public async Task Given_IHaveAValidUser_UpdateLevelRequest_AndABadUserGuid_WhenIUpdateUserLevel_ThenANotFoundExceptionShouldBeThrown()
        {
            var levelId = (await db.UserLevels.FirstOrDefaultAsync(x => x.Name == "Regular")).Id;
            var command = new User_UpdateLevel { LevelId = levelId, UserId = Guid.Empty };
            var SUT = new UserService(db);

            Assert.ThrowsAsync<NotFoundException>(() => SUT.UpdateUserLevel(command));
        }

        [Test]
        public async Task Given_IHaveAValidUser_UpdateLevelRequest_AndABadLevelGuid_WhenIUpdateUserLevel_ThenANotFoundExceptionShouldBeThrown()
        {
            var userId = (await db.Users.FirstOrDefaultAsync(x => x.FirstName == "Dan")).Id;
            var command = new User_UpdateLevel { LevelId = Guid.Empty, UserId = userId };
            var SUT = new UserService(db);

            Assert.ThrowsAsync<NotFoundException>(() => SUT.UpdateUserLevel(command));
        }
        #endregion
    }
}
