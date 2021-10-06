using MiniURL.Core.Exceptions;
using MiniURL.Core.ValueObjects;
using NUnit.Framework;

namespace MiniURL.Tests.Core.ValueObjects
{
    public class UrlTests
    {
        [Test]
        public void Given_IHaveAValidUrl_WhenCreatingAUrl_ThenThePropertiesAreAsExpected()
        {
            var SUT = new Url("http://www.xmen.com/?val=123");
            Assert.That(SUT.Scheme, Is.EqualTo("http"));
            Assert.That(SUT.Host, Is.EqualTo("www.xmen.com"));
            Assert.That(SUT.Path, Is.EqualTo("/"));
            Assert.That(SUT.Query, Is.EqualTo("?val=123"));
            Assert.That(SUT.ToString(), Is.EqualTo($"http://www.xmen.com/?val=123"));
        }

        [Test]
        public void Given_IHaveAUrlWithNoScheme_WhenCreatingAUrl_ThenAnInvalidUrlExceptionIsThrown()
        {
            Assert.Throws<InvalidUrlException>(() => new Url("www.xmen.com"));
        }

        [Test]
        public void Given_IHaveAUrlWithNoHost_WhenCreatingAUrl_ThenAnInvalidUrlExceptionIsThrown()
        {
            Assert.Throws<InvalidUrlException>(() => new Url("https://"));
        }

        [Test]
        public void Given_IHaveAGenericStiring_WhenCreatingAUrl_ThenAnInvalidUrlExceptionIsThrown()
        {
            Assert.Throws<InvalidUrlException>(() => new Url("Hello my name is tim"));
        }

    }
}