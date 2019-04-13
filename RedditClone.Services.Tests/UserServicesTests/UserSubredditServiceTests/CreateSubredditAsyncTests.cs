using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Tests.Common;
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

        [Theory]
        [InlineData("Subreddit name")]
        public async Task WithModelWithName_ShouldCreateNewSubredditWithCorrectName(string subredditName)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var model = new SubredditCreationBindingModel()
            {
                Name = subredditName
            };

            await this.CallCreateSubredditAsync(unitOfWork, model);
            var dbSubreddit = unitOfWork.Subreddits.Find(s => s.Name == subredditName).First();

            Assert.Equal(subredditName, dbSubreddit.Name);
        }

        [Theory]
        [InlineData("Description")]
        public async Task WithModelWithDescription_ShouldCreateNewSubredditWithCorrectDescription(string description)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var model = new SubredditCreationBindingModel()
            {
                Description = description
            };

            await this.CallCreateSubredditAsync(unitOfWork, model);
            var dbSubreddit = unitOfWork.Subreddits.Find(s => s.Description == description).First();

            Assert.Equal(description, dbSubreddit.Description);
        }

        [Fact]
        public async Task WithModelWithUserWithId_ShouldCreateNewSubredditWithCorrectAuthorId()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            unitOfWork.Users.Add(dbUser);
            unitOfWork.Complete();
            var model = new SubredditCreationBindingModel();

            var dbUserId = dbUser.Id;
            await this.CallCreateSubredditAsyncWithUser(unitOfWork, model, dbUserId);
            var dbSubreddit = unitOfWork.Subreddits.Find(s => s.AuthorId == dbUserId).First();
            var modelAuthorId = dbSubreddit.AuthorId;

            Assert.Equal(dbUserId, modelAuthorId);
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
