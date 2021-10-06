using MiniURL.Core.Exceptions;
using MiniURL.Core.ValueObjects;
using NUnit.Framework;

namespace MiniURL.Tests.Core.ValueObjects
{
    public class EmailTests
    {
        [Test]
        public void Given_IHaveAValidEmailAddress_WhenICreateAnEmail_ThenThePropertiesShouldBePopulated()
        {
            var SUT = new Email("daniel.layland@test.com");
            Assert.That(SUT.Domain, Is.EqualTo("test.com"));
            Assert.That(SUT.Username, Is.EqualTo("daniel.layland"));
            Assert.That(SUT.ToString(), Is.EqualTo("daniel.layland@test.com"));
        }

        [Test]
        public void Given_IHaveARandomString_WhenICreateAnEmail_ThenItShouldThrowAnInvalidEmailException()
        {
            Assert.Throws<InvalidEmailException>(() => new Email("asdapoilfhoiuh"));
        }

        [Test]
        public void Given_IHaveAnEmailWithNoAt_WhenICreateAnEmail_ThenItShouldThrowAnInvalidEmailException()
        {
            Assert.Throws<InvalidEmailException>(() => new Email("daniel.laylandtest.com"));
        }
    }
}
