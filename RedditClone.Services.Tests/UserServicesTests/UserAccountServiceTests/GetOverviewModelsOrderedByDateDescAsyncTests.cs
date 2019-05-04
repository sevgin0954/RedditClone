using Moq;
using RedditClone.Models;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserAccountServiceTests.UserPostServiceTests
{
    public class GetOverviewModelsOrderedByDateDescAsyncTests : BaseUserAccountServiceTest
    {
        [Fact]
        public async Task WithoutActivity_ShouldReturnEmptyCollection()
        {
            var dbUser = new User();

            var models = await this.CallGetOverviewModelAsync(dbUser);

            Assert.Empty(models);
        }

        [Fact]
        public async Task WithIncorrectUserId_ShouldReturnNull()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var service = this.GetService(unitOfWork);

            var dbUserIncorrectId = Guid.NewGuid().ToString();
            var models = await service.GetOverviewModelsOrderedByDateDescAsync(dbUserIncorrectId);

            Assert.Null(models);
        }

        [Fact]
        public async Task WithUserWithCreatedPostAndComment_ShouldReturnCorrectModelsCount()
        {
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbUser = new User();
            this.AddEntitiesToUser(dbUser, dbComment, dbPost);

            var models = await this.CallGetOverviewModelAsync(dbUser);
            var modelsCount = models.Count();

            Assert.Equal(2, modelsCount);
        }

        [Fact]
        public async Task WithUserWithCreatedPostsAndComments_ShouldReturnModelsInDescendingOrderByPostDate()
        {
            var dbUser = new User();
            var dbComment1 = new Comment();
            var dbComment2 = new Comment();
            var dbPost1 = new Post();
            var dbPost2 = new Post();
            this.AddEntitiesToUser(dbUser, dbComment1, dbComment2, dbPost1, dbPost2);

            var initialDatetime = DateTime.UtcNow;
            dbComment1.PostDate = initialDatetime.AddDays(2);
            dbPost1.PostDate = initialDatetime.AddDays(1).AddSeconds(1);
            dbComment2.PostDate = initialDatetime.AddDays(1);
            dbPost2.PostDate = initialDatetime;

            var models = await this.CallGetOverviewModelAsync(dbUser);
            var modelsAsArray = models.ToArray();

            Assert.True(DateTime.Compare(modelsAsArray[0].PostDate, dbComment1.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[1].PostDate, dbPost1.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[2].PostDate, dbComment2.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[3].PostDate, dbPost2.PostDate) == 0);
        }

        private void AddEntitiesToUser(User user, Comment comment1, Comment comment2, Post post1, Post post2)
        {
            this.AddEntitiesToUser(user, comment1, post1);
            this.AddEntitiesToUser(user, comment2, post2);
        }

        private void AddEntitiesToUser(User user, Comment comment, Post post)
        {
            user.Comments.Add(comment);
            user.Posts.Add(post);
        }

        private async Task<IEnumerable<UserActionViewModel>> CallGetOverviewModelAsync(User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var models = await service.GetOverviewModelsOrderedByDateDescAsync(user.Id);
            return models;
        }
    }
}