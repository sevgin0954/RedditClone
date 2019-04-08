using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Common.Constants;
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
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            var mockedUserPrinciple = new Mock<ClaimsPrincipal>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();
            var mockedResponseCookies = new Mock<IResponseCookies>();

            await service.GetOrderedPostsAsync(
                mockedUserPrinciple.Object, 
                mockedRequestCookieCollection.Object, 
                mockedResponseCookies.Object);

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = WebConstants.CookieDefaultValuePostSortType;

            mockedResponseCookies
                .Verify(rc => rc.Append(postSortTypeKey, postSortTypeValue, It.IsAny<CookieOptions>()), Times.Once);

            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;

            mockedResponseCookies
                .Verify(rc => rc.Append(postTimeFrameKey, postTimeFrameValue, It.IsAny<CookieOptions>()), Times.Exactly(2));
        }
    }
}
