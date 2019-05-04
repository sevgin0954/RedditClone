using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Services.Tests.Common;
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

            var timeFrameKey = WebConstants.CookieKeyPostTimeFrameType;
            var timeFrameIncorrectValue = Guid.NewGuid().ToString();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            CommonTestMethods
                .SutupMockedRequestCookieCollection(mockedRequestCookies, timeFrameKey, timeFrameIncorrectValue);

            var timeFrameType = service.GetPostTimeFrameTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultTimeFrameType = WebConstants.CookieDefaultValuePostTimeFrameType;

            Assert.Equal(defaultTimeFrameType, timeFrameType.ToString());
        }

        [Fact]
        public void WithCorrectPostTimeFrameTypeCookie_ShouldReturnCorrectPostTimeFrameType()
        {
            var service = this.GetService();

            var timeFrameKey = WebConstants.CookieKeyPostTimeFrameType;
            var expectedTimeFrameType = TimeFrameType.PastMonth;

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            CommonTestMethods
                .SutupMockedRequestCookieCollection(mockedRequestCookies, timeFrameKey, expectedTimeFrameType.ToString());

            var timeFrameType = service.GetPostTimeFrameTypeFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(expectedTimeFrameType, expectedTimeFrameType);
        }
    }
}
