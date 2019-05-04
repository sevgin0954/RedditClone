using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.BindingModels;
using RedditClone.Services.Tests.Common;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserCommentServiceTests
{
    public class AddCommentToPostAsyncTests : BaseAddCommentToPostAsyncTest
    {
        [Fact]
        public async Task WithModelWithIncorrectPostId_ShouldReturnFalse()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var incorrectPostId = Guid.NewGuid().ToString();
            var model = new CommentBindingModel()
            {
                SourceId = incorrectPostId
            };

            var result = await this.CallAddCommentToPostAsync(unitOfWork, model);

            Assert.False(result);
        }

        [Fact]
        public async Task WithCorrectModel_ShouldReturnTrue()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost = new Post();
            var dbUser = new User();
            this.AddEntitiesToUnitOfWork(unitOfWork, dbPost, dbUser);

            var description = Guid.NewGuid().ToString();
            var model = this.CreateCommentModel(dbPost.Id, description);

            var result = await this.CallAddCommentToPostAsync(unitOfWork, model);

            Assert.True(result);
        }

        [Fact]
        public async Task WithCorrectModel_ShouldAddNewCommentToPost()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost = new Post();
            var dbUser = new User();
            this.AddEntitiesToUnitOfWork(unitOfWork, dbPost, dbUser);

            var description = Guid.NewGuid().ToString();
            var model = this.CreateCommentModel(dbPost.Id, description);

            await this.CallAddCommentToPostAsync(unitOfWork, model);
            var dbPostCommentsCount = dbPost.Comments.Count;

            Assert.Equal(1, dbPostCommentsCount);
        }

        private CommentBindingModel CreateCommentModel(string postId, string description)
        {
            var model = new CommentBindingModel()
            {
                SourceId = postId,
                Description = description
            };

            return model;
        }

        private void AddEntitiesToUnitOfWork(IRedditCloneUnitOfWork unitOfWork, Post post, User user)
        {
            unitOfWork.Posts.Add(post);
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();
        }

        private async Task<bool> CallAddCommentToPostAsync(IRedditCloneUnitOfWork unitOfWork, CommentBindingModel model)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;

            var result = await service.AddCommentToPostAsync(claimsPrincipal, model);
            return result;
        }
    }
}
