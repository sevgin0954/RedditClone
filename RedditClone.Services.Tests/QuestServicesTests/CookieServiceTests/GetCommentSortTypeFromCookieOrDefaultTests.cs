using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Services.Tests.Common;
using System;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.CookieServiceTests
{
    public class GetCommentSortTypeFromCookieOrDefaultTests : BaseCookieServiceTest
    {
        [Fact]
        public void WithoutCommentSortTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var sortType = service.GetCommentSortTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultSortType = WebConstants.CookieDefaultValueCommentSortType;

            Assert.Equal(defaultSortType, sortType.ToString());
        }

        [Fact]
        public void WithIncorrectCommentSortTypeCookie_ShouldReturnDefault()
        {
            var service = this.GetService();

            var sortTypeKey = WebConstants.CookieKeyCommentSortType;
            var sortTypeIncorrectValue = Guid.NewGuid().ToString();

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            CommonTestMethods
                .SutupMockedRequestCookieCollection(mockedRequestCookies, sortTypeKey, sortTypeIncorrectValue);

            var sortType = service.GetCommentSortTypeFromCookieOrDefault(mockedRequestCookies.Object);
            var defaultSortType = WebConstants.CookieDefaultValueCommentSortType;

            Assert.Equal(defaultSortType, sortType.ToString());
        }

        [Fact]
        public void WithCorrectCommentSortTypeCookie_ShouldReturnCorrectCommentSortType()
        {
            var service = this.GetService();

            var sortTypeKey = WebConstants.CookieKeyCommentSortType;
            var correctSortType = CommentSortType.Controversial;

            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            CommonTestMethods
                .SutupMockedRequestCookieCollection(mockedRequestCookies, sortTypeKey, correctSortType.ToString());

            var sortType = service.GetCommentSortTypeFromCookieOrDefault(mockedRequestCookies.Object);

            Assert.Equal(correctSortType, sortType);
        }
    }
}
