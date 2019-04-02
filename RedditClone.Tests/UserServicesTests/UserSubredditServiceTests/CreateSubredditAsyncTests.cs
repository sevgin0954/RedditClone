using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.UserServicesTests.UserSubredditServiceTests
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

            var dbSubreddit = unitOfWork.Subreddits.Find(s => s.Name == subredditName);

            Assert.NotEmpty(dbSubreddit);
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

            var dbSubreddit = unitOfWork.Subreddits.Find(s => s.Description == description);

            Assert.NotEmpty(dbSubreddit);
        }

        public async Task<bool> CallCreateSubredditAsync(
            IRedditCloneUnitOfWork unitOfWork, 
            SubredditCreationBindingModel model)
        {
            var mockedUserManager = this.GetMockedUserManager();
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var result = await service.CreateSubredditAsync(model, mockedClaimsPrincipal.Object);

            return result;
        }
    }
}
