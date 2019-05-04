using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Services.Tests.Common;
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

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var sortTypeIncorrectValue = Guid.NewGuid().ToString();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
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

            var sortTypeKey = WebConstants.CookieKeyPostSortType;
            var correctSortType = PostSortType.Controversial;

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            CommonTestMethods.SutupMockedRequestCookieCollection(
                mockedRequestCookies, sortTypeKey, correctSortType.ToString());

            var sortType = service.GetPostSortTypeFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(correctSortType, sortType);
        }
    }
}
