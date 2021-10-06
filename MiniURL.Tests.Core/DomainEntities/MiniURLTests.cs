using System;
using NUnit.Framework;
using System.Linq;
using MiniURL.Core.Models;

namespace MiniURL.Tests.Core.DomainEntities
{
    public class MiniURLTests
    {
        readonly static UserLevel guest = new("Guest", "some description", 1);

        [Test]
        public void Given_IHaveAUserWithAMiniURL_WhenIGenerateANewReference_ThenTheReferenceIsUpdated()
        {
            var SUT = new User("Mr", "Dan", "Test", "dan.test@w.com", guest);
            SUT.CreateNewMiniUrl("https://www.google.com");

            var url = SUT.MiniUrls.Last();
            var reference = url.Reference;
            url.GenerateNewReference();
            Assert.That(reference, Is.Not.EqualTo(url.Reference));
        }
    }
}
