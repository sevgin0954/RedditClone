using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.TimeFrameTypes;
using System;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.CookieServiceTests
{
    public class GetPostTimeFrameTypeFromCookieOrDefaultTests : BaseCookieServiceTest
    {
        [Fact]
        public void WithoutPostTimeFrameTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();
            var mockedRequestCookies = new Mock<IRequestCookieCollection>();

            var timeFrameType = service.GetPostTimeFrameTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultTimeFrameType = WebConstants.CookieDefaultValuePostTimeFrameType;

            Assert.Equal(defaultTimeFrameType, timeFrameType.ToString());
        }

        [Fact]
        public void WithIncorrectPostTimeFrameTypeTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postPostShowTimeKey = WebConstants.CookieKeyPostTimeFrameType;
            var timeFrameTypeIncorrectValue = Guid.NewGuid().ToString();
            mockedRequestCookies.SetupGet(rc => rc[postPostShowTimeKey])
                .Returns(timeFrameTypeIncorrectValue);

            var timeFrameType = service.GetPostTimeFrameTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultTimeFrameType = WebConstants.CookieDefaultValuePostTimeFrameType;

            Assert.Equal(defaultTimeFrameType, timeFrameType.ToString());
        }

        [Fact]
        public void WithCorrectPostTimeFrameTypeCookie_ShouldReturnCorrectPostTimeFrameType()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postPostShowTimeKey = WebConstants.CookieKeyPostTimeFrameType;
            var expectedTimeFrameType = TimeFrameType.PastMonth;
            mockedRequestCookies.SetupGet(rc => rc[postPostShowTimeKey])
                .Returns(expectedTimeFrameType.ToString());

            var timeFrameType = service.GetPostTimeFrameTypeFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(expectedTimeFrameType, expectedTimeFrameType);
        }
    }
}
