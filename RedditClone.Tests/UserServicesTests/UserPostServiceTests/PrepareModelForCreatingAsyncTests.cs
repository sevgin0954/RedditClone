using Moq;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Tests.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.UserServicesTests.UserPostServiceTests
{
    public class PrepareModelForCreatingAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithNullSubredditId_ShouldReturnModel()
        {
            var dbUser = new User();

            var model = await this.CallPrepareModelForCreatingAsync(dbUser, null);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task WithUserWithId_ShouldReturnModelWithCorrectAuthorIdAsync()
        {
            var dbUser = new User();

            var model = await this.CallPrepareModelForCreatingAsync(dbUser, null);
            var modelAuthorId = model.AuthorId;
            var dbUserId = dbUser.Id;

            Assert.Equal(dbUserId, modelAuthorId);
        }

        private async Task<CreationPostBindingModel> CallPrepareModelForCreatingAsync(User user, string subredditId)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var mockedUserManager = this.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserAsync(mockedUserManager, user);

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var model = await service.PrepareModelForCreatingAsync(mockedClaimsPrincipal.Object, subredditId);

            return model;
        }
    }
}
