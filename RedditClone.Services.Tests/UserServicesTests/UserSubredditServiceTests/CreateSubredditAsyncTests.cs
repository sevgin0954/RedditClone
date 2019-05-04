using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Services.Tests.Common;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserSubredditServiceTests
{
    public class CreateSubredditAsyncTests : BaseUserSubredditServiceTest
    {
        [Fact]
        public async Task WithCorrectModel_ShouldReturnTrue()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var model = new SubredditCreationBindingModel();

            var result = await this.CallCreateSubredditAsync(unitOfWork, model);

            Assert.True(result);
        }

        [Fact]
        public async Task WithModelWithAlreadyExistingName_ShouldReturnFalse()
        {
            const string subredditName = "SubredditName";

            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbSubreddit = new Subreddit()
            {
                Name = subredditName
            };
            unitOfWork.Subreddits.Add(dbSubreddit);
            unitOfWork.Complete();
            var model = new SubredditCreationBindingModel()
            {
                Name = subredditName
            };

            var result = await this.CallCreateSubredditAsync(unitOfWork, model);

            Assert.False(result);
        }

        [Fact]
        public async Task WithModelAndUserWithId_ShouldCreateNewSubredditWithCorrectAuthorId()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            unitOfWork.Users.Add(dbUser);
            unitOfWork.Complete();
            var model = new SubredditCreationBindingModel();
            
            await this.CallCreateSubredditAsyncWithUser(unitOfWork, model, dbUser.Id);
            var dbSubreddits = await unitOfWork.Subreddits.FindAsync(s => s.AuthorId == dbUser.Id);
            var dbFirstSubreddit = dbSubreddits.First();
            var modelAuthorId = dbFirstSubreddit.AuthorId;

            Assert.Equal(dbUser.Id, modelAuthorId);
        }

        private async Task<bool> CallCreateSubredditAsync(
            IRedditCloneUnitOfWork unitOfWork, 
            SubredditCreationBindingModel model)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var result = await service.CreateSubredditAsync(model, mockedClaimsPrincipal.Object);

            return result;
        }

        private async Task<bool> CallCreateSubredditAsyncWithUser(
            IRedditCloneUnitOfWork unitOfWork,
            SubredditCreationBindingModel model,
            string userId)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserId(mockedUserManager, userId);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var result = await service.CreateSubredditAsync(model, mockedClaimsPrincipal.Object);

            return result;
        }
    }
}
