using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using System;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.CookieServiceTests
{
    public class GetPostSortTypeFromCookieOrDefaultTests : BaseCookieServiceTest
    {
        [Fact]
        public void WithoutPostSortTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();
            var mockedRequestCookies = new Mock<IRequestCookieCollection>();

            var sortType = service.GetPostSortTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultSortType = WebConstants.CookieDefaultValuePostSortType;

            Assert.Equal(defaultSortType, sortType.ToString());
        }

        [Fact]
        public void WithIncorrectPostSortTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var sortTypeIncorrectValue = Guid.NewGuid().ToString();
            mockedRequestCookies.SetupGet(rc => rc[postSortTypeKey])
                .Returns(sortTypeIncorrectValue);

            var sortType = service.GetPostSortTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultSortType = WebConstants.CookieDefaultValuePostSortType;

            Assert.Equal(defaultSortType, sortType.ToString());
        }

        [Fact]
        public void WithCorrectPostSortTypeCookie_ShouldReturnCorrectPostSortType()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var correctSortType = SortType.Controversial;
            mockedRequestCookies.SetupGet(rc => rc[postSortTypeKey])
                .Returns(correctSortType.ToString());

            var sortType = service.GetPostSortTypeFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(correctSortType, sortType);
        }
    }
}
