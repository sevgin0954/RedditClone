using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using System;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.CookieServiceTests
{
    public class GetPostShowTimeFrameFromCookieOrDefaultTests : BaseCookieServiceTest
    {
        [Fact]
        public void WithoutPostShowTimeFrameCookie_ShouldReturnDefault()
        {
            var service = this.GetService();
            var mockedRequestCookies = new Mock<IRequestCookieCollection>();

            var showTimeFrame = service.GetPostShowTimeFrameFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultShowTimeFrame = WebConstants.CookieDefaultValuePostShowTimeFrame;

            Assert.Equal(defaultShowTimeFrame, showTimeFrame.ToString());
        }

        [Fact]
        public void WithIncorrectPostShowTimeFrameCookie_ShouldReturnDefault()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postPostShowTimeKey = WebConstants.CookieKeyPostShowTimeFrame;
            var showTimeFrameIncorrectValue = Guid.NewGuid().ToString();
            mockedRequestCookies.SetupGet(rc => rc[postPostShowTimeKey])
                .Returns(showTimeFrameIncorrectValue);

            var showTimeFrame = service.GetPostShowTimeFrameFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultShowTimeFrame = WebConstants.CookieDefaultValuePostShowTimeFrame;

            Assert.Equal(defaultShowTimeFrame, showTimeFrame.ToString());
        }

        [Fact]
        public void WithCorrectPostShowTimeFrameCookie_ShouldReturnCorrectPostShowTimeFrame()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postPostShowTimeKey = WebConstants.CookieKeyPostShowTimeFrame;
            var correctShowTimeFrame = PostShowTimeFrame.PastMonth;
            mockedRequestCookies.SetupGet(rc => rc[postPostShowTimeKey])
                .Returns(correctShowTimeFrame.ToString());

            var showTimeFrame = service.GetPostShowTimeFrameFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(correctShowTimeFrame, showTimeFrame);
        }
    }
}
