using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Services.Tests.Common;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public class CreatePostAsync : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithModelWithSelectedSubredditId_ShouldAddNewPostToDatabase()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.AddUserToDatabase(unitOfWork, dbUser);
            this.AddSubredditToDatabase(unitOfWork, dbSubreddit);

            var model = new PostCreationBindingModel()
            {
                SelectedSubredditId = dbSubreddit.Id
            };
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task WithUserWithIdAndSelectedSubredditId_ShouldAddNewPostWithCorrectAuthorId()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.AddUserToDatabase(unitOfWork, dbUser);
            this.AddSubredditToDatabase(unitOfWork, dbSubreddit);

            var model = new PostCreationBindingModel()
            {
                SelectedSubredditId = dbSubreddit.Id
            };
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            var dbPost = this.GetPostWithAuthorId(unitOfWork, dbUser.Id);

            Assert.Equal(dbPost.AuthorId, dbUser.Id);
        }

        [Theory]
        [InlineData("Post title")]
        public async Task WithModelWithTitleAndSelectedSubredditId_ShouldAddNewPostWithCorrectTitle(string titleName)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.AddUserToDatabase(unitOfWork, dbUser);
            this.AddSubredditToDatabase(unitOfWork, dbSubreddit);

            var model = new PostCreationBindingModel()
            {
                SelectedSubredditId = dbSubreddit.Id,
                Title = titleName
            };
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            var dbPost = this.GetPostWithAuthorId(unitOfWork, dbUser.Id);

            Assert.Equal(titleName, dbPost.Title);
        }

        [Theory]
        [InlineData("Description")]
        public async Task WithModelWithDescriptionAndSelectedSubredditId_ShouldAddNewPostWithCorrectDescription(string description)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.AddUserToDatabase(unitOfWork, dbUser);
            this.AddSubredditToDatabase(unitOfWork, dbSubreddit);

            var model = new PostCreationBindingModel()
            {
                SelectedSubredditId = dbSubreddit.Id,
                Description = description
            };
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            var dbPost = this.GetPostWithAuthorId(unitOfWork, dbUser.Id);

            Assert.Equal(description, dbPost.Description);
        }

        [Fact]
        public async Task WithModelWithoutSelectedSubredditId_ShouldReturnFalse()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            this.AddUserToDatabase(unitOfWork, dbUser);

            var model = new PostCreationBindingModel();
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            Assert.False(result);
        }

        [Fact]
        public async Task WithModelWithoutSelectedSubredditId_ShouldNotAddNewPost()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            this.AddUserToDatabase(unitOfWork, dbUser);

            var model = new PostCreationBindingModel();
            var result = await this.CallCreatePostAsync(unitOfWork, model, dbUser.Id);

            var dbPosts = unitOfWork.Posts.GetAll();

            Assert.Empty(dbPosts);
        }

        private void AddUserToDatabase(IRedditCloneUnitOfWork unitOfWork, User user)
        {
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();
        }

        private void AddSubredditToDatabase(IRedditCloneUnitOfWork unitOfWork, Subreddit subreddit)
        {
            unitOfWork.Subreddits.Add(subreddit);
            unitOfWork.Complete();
        }

        private Post GetPostWithAuthorId(IRedditCloneUnitOfWork unitOfWork, string authorId)
        {
            var dbPosts = unitOfWork.Posts.Find(p => p.AuthorId == authorId);
            return dbPosts.First();
        }

        private async Task<bool> CallCreatePostAsync(
            IRedditCloneUnitOfWork unitOfWork, 
            PostCreationBindingModel model, 
            string userId)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserId(mockedUserManager, userId);

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            
            var mockedClaimsPricipal = new Mock<ClaimsPrincipal>();
            var result = await service.CreatePostAsync(mockedClaimsPricipal.Object, model);

            return result;
        }
    }
}
