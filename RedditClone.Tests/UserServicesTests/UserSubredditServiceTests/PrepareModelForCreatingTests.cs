using Moq;
using RedditClone.Models;
using RedditClone.Tests.Common;
using System.Security.Claims;
using Xunit;

namespace RedditClone.Tests.UserServicesTests.UserSubredditServiceTests
{
    public class PrepareModelForCreatingTests : BaseUserSubredditServiceTest
    {
        [Fact]
        public void WithUser_ShouldReturnModelWithCorrectAuthorId()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            unitOfWork.Users.Add(dbUser);
            unitOfWork.Complete();

            var mockedUserManager = this.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserId(mockedUserManager, dbUser.Id);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            var model = service.PrepareModelForCreating(mockedClaimsPrincipal.Object);

            Assert.Equal(dbUser.Id, model.AuthorId);
        }
    }
}
