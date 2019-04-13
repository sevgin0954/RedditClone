using Moq;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using RedditClone.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Common.Constants;
using RedditClone.Services.Tests.Common;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public class PrepareModelForCreatingAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithUserWithNullSubredditId_ShouldReturnModel()
        {
            var dbUser = new User();

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);

            Assert.NotNull(model);
        }

        [Fact]
        public async Task WithUserWithCreatedSubredditWithSubredditId_ShouldReturnModelWithCorrectSelectedSubredditId()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var modelSelectedSubredditId = model.SelectedSubredditId;
            var dbSubredditId = dbSubreddit.Id;

            Assert.Equal(dbSubredditId, modelSelectedSubredditId);
        }

        [Fact]
        public async Task WithUserWithSubscribedSubredditWithSubredditId_ShouldReturnModelWithCorrectSelectedSubredditId()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var modelSelectedSubredditId = model.SelectedSubredditId;

            Assert.Equal(dbSubreddit.Id, modelSelectedSubredditId);
        }

        [Fact]
        public async Task WithUserWithoutSubreddits_ShouldReturnModelWithTwoInitialSubreddits()
        {
            var dbUser = new User();

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var selectLists = model.Subreddits;
            var selectListsCount = selectLists.Count;
            
            Assert.Equal(2, selectListsCount);
        }

        [Fact]
        public async Task WithUserWithCreatedSubreddit_ShouldReturnModelWithCorrectSubredditsCount()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var selectList = model.Subreddits;
            var selectListItemsCount = selectList.Count();

            Assert.Equal(2, selectListItemsCount);
        }

        [Fact]
        public async Task WithUserWithSubscribedSubreddit_ShouldReturnModelWithCorrectSubredditsCount()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var selectList = model.Subreddits;
            var selectListItemsCount = selectList.Count();

            Assert.Equal(2, selectListItemsCount);
        }

        [Fact]
        public async Task WithUserWithCreatedAndSubscribedSubreddits_ShouldReturnTwoSubredditsCount()
        {
            var dbUser = new User();
            var dbSubreddit1 = new Subreddit();
            var dbSubreddit2 = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit1);
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit2);

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var selectList = model.Subreddits;
            var selectListItemsCount = selectList.Count();

            Assert.Equal(2, selectListItemsCount);
        }

        [Fact]
        public async Task WithUserWithCreatedSubredditWithSubredditId_ShoudReturnModelWithCorrectSubredditValue()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit);
            
            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.Equal(dbSubreddit.Id, firstSubreddit.Value);
        }

        [Fact]
        public async Task WithUserWithSubscribedSubredditWithSubredditId_ShoudReturnModelWithCorrectSubredditValue()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);
            
            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.Equal(dbSubreddit.Id, firstSubreddit.Value);
        }

        [Fact]
        public async Task WithUserWithCreatedSubredditWithSubredditId_ShoudReturnModelWithTrueSubredditSelectedFlag()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit);
            
            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.True(firstSubreddit.Selected);
        }

        [Fact]
        public async Task WithUserWithSubscribedSubredditWithSubredditId_ShoudReturnModelWithTrueSubredditSelectedFlag()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithSubredditId(dbUser, dbSubreddit);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.True(firstSubreddit.Selected);
        }

        [Fact]
        public async Task WithUserWithCreatedSubredditWithoutSubredditId_ShoudReturnModelWithFalseSubredditSelectedFlag()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            dbUser.CreatedSubreddits.Add(dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.False(firstSubreddit.Selected);
        }

        [Fact]
        public async Task WithUserWithSuscribedSubredditWithoutSubredditId_ShoudReturnModelWithFalseSubredditSelectedFlag()
        {
            var dbUser = new User();
            var dbSubreddit = new Subreddit();
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var firstSubreddit = this.GetFirstNonEmptySubredditFromModel(model);

            Assert.False(firstSubreddit.Selected);
        }

        [Fact]
        public async Task WithUserWithoutSubreddits_ShouldReturnModelWithCorrectGroupsNames()
        {
            var dbUser = new User();

            var model = await this.CallPrepareModelForCreatingAsyncWithNullSubredditId(dbUser);
            var selectLists = model.Subreddits;
            var groups = selectLists.Select(sl => sl.Group);

            Assert.Contains(groups, g => g.Name == ModelsConstants.SelectListGroupNameCreatedSubreddits);
            Assert.Contains(groups, g => g.Name == ModelsConstants.SelectListGroupNameSubscribedSubreddits);
        }

        private void SubscribeUserToSubreddit(User user, Subreddit subreddit)
        {
            var dbUserSubreddit = new UserSubreddit()
            {
                User = user,
                Subreddit = subreddit
            };
            user.SubscribedSubreddits.Add(dbUserSubreddit);
        }

        private SelectListItem GetFirstNonEmptySubredditFromModel(PostCreationBindingModel model)
        {
            var modelSubreddits = model.Subreddits;
            var firstSubreddit = modelSubreddits
                .Where(s => s.Text != ModelsConstants.SelectListItemNameEmpty)
                .First();

            return firstSubreddit;
        }

        private async Task<PostCreationBindingModel> CallPrepareModelForCreatingAsyncWithNullSubredditId(
            User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var model = await this.CallPrepareModelForCreatingAsync(unitOfWork, user, null);

            return model;
        }

        private async Task<PostCreationBindingModel> CallPrepareModelForCreatingAsyncWithSubredditId(
            User user, 
            Subreddit subreddit)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Subreddits.Add(subreddit);
            unitOfWork.Complete();
            
            var model = await this.CallPrepareModelForCreatingAsync(unitOfWork, user, subreddit.Id);

            return model;
        }

        private async Task<PostCreationBindingModel> CallPrepareModelForCreatingAsync(
            IRedditCloneUnitOfWork unitOfWork,
            User user,
            string subredditId)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserId(mockedUserManager, user.Id);

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var model = await service.PrepareModelForCreatingAsync(mockedClaimsPrincipal.Object, subredditId);

            return model;
        }
    }
}
