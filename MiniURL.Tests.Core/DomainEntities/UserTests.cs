using MiniURL.Core.Exceptions;
using MiniURL.Core.Models;
using MiniURL.Core.ValueObjects;
using NUnit.Framework;
using System;
using System.Linq;

namespace MiniURL.Tests.Core.DomainEntities
{
    public class UserTests
    {
        readonly static UserLevel guest = new("Guest", "some description", 1);
        readonly static UserLevel regular = new("Regular", "some description", 5);

        [Test]
        public void Given_IHaveAValidUserLevel_WhenICreateAUser_ThenTheUserIsCreated()
        {
            var SUT = new User("Mr", "Test", "Account", "daniel.layland@test.com", new UserLevel("Guest", "some description", 1));
            Assert.That(SUT.Level, Is.Not.Null);
            Assert.That(SUT.Level.Id, Is.EqualTo(SUT.LevelId));
            Assert.That(SUT.Level.Id, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void Given_IDontHaveAValidUserLevel_WhenICreateAUser_ThenAnArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new User("Mr", "Test", "Account", "daniel.layland@test.com", null));
        }

        [Test]
        public void Given_IHaveAValidUserLevel_WhenIUpdateAUsersLevel_ThenTheUsersLevelShouldBeUpdated_AndAnyURLSDurationExtended()
        {
            var SUT = new User("Mr", "Test", "Account", "daniel.layland@test.com", guest);
            SUT.CreateNewMiniUrl("https://www.google.com");
            SUT.UpdateLevel(regular);

            Assert.That(SUT.Level.Name, Is.EqualTo("Regular"));
            Assert.That(SUT.MiniUrls.First().ExpiresOn, Is.GreaterThan(DateTimeOffset.UtcNow.AddDays(1)));
        }

        [Test]
        public void Given_IHaveAUser_WhenIUpdateEmailToValidAddress_ThenTheUsersEmailShouldBeUpdated()
        {
            var SUT = new User("Mr", "Test", "Account", "daniel.layland@test.com", guest);
            SUT.UpdateEmail("dan.test@e.com");
            string email = SUT.Email;

            Assert.That(email, Is.EqualTo("dan.test@e.com"));
        }

        [Test]
        public void Given_IHaveAUser_WhenIUpdateEmailToInvalidAddress_ThenAnInvalidEmailExceptionShouldBeThrown()
        {
            var SUT = new User("Mr", "Test", "Account", "daniel.layland@test.com", guest);
            Assert.Throws<InvalidEmailException>(() => SUT.UpdateEmail("hello"));
        }
    }
}
