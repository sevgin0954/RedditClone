using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Tests.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.UserServicesTests.UserPostServiceTests
{
    public class GetOrderedPostsAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithoutCookies_ShouldSetDefaultCookies()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            await this.CallGetOrderedPostsAsync(mockedResponseCookies, mockedRequestCookieCollection);

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = WebConstants.CookieDefaultValuePostSortType;
            mockedResponseCookies
                .Verify(rc => rc.Append(postSortTypeKey, postSortTypeValue, It.IsAny<CookieOptions>()), Times.Once);

            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;
            mockedResponseCookies
                .Verify(rc => rc.Append(postTimeFrameKey, postTimeFrameValue, It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        [Fact]
        public async Task WithNotTimeDependentSortType_ShouldReturnModelWithNullPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.New.ToString());

            var model = await this.CallGetOrderedPostsAsync(mockedResponseCookies, mockedRequestCookieCollection);

            Assert.Null(model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithTimeDependentSortType_ShouldReturnModelWithPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.Controversial.ToString());

            var model = await this.CallGetOrderedPostsAsync(mockedResponseCookies, mockedRequestCookieCollection);

            Assert.NotNull(model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithTimeDependentSortTypeAndPostShowTimeFrameCookie_ShouldReturnModelWithCorrectPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.Controversial.ToString());

            var postShowTimeFrame = PostShowTimeFrame.PastMonth;
            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            mockedRequestCookieCollection.SetupGet(r => r[postTimeFrameKey])
                .Returns(postShowTimeFrame.ToString());

            var model = await this.CallGetOrderedPostsAsync(mockedResponseCookies, mockedRequestCookieCollection);

            Assert.Equal(postShowTimeFrame, model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithNotTimeDependentSortType_ShouldSetDefaultPostShowTimeFrameCookie()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.New.ToString());

            var model = await this.CallGetOrderedPostsAsync(mockedResponseCookies, mockedRequestCookieCollection);

            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;
            mockedResponseCookies
                .Verify(rc => rc.Append(postTimeFrameKey, postTimeFrameValue, It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsync(
            Mock<IResponseCookies> mockedResponseCookies, 
            Mock<IRequestCookieCollection> mockedRequestCookieCollection)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            var mockedUserPrinciple = new Mock<ClaimsPrincipal>();

            var model = await service.GetOrderedPostsAsync(
                mockedUserPrinciple.Object,
                mockedRequestCookieCollection.Object,
                mockedResponseCookies.Object);

            return model;
        }
    }
}
