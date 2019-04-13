using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.BindingModels;
using RedditClone.Services.Tests.Common;
using System;
using System.Linq;
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
            var randomPostId = Guid.NewGuid().ToString();
            var model = new CommentBindingModel()
            {
                SourceId = randomPostId
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
            unitOfWork.Posts.Add(dbPost);
            unitOfWork.Users.Add(dbUser);
            unitOfWork.Complete();

            var model = this.CreateCorrectCommentModel(dbPost.Id);

            var result = await this.CallAddCommentToPostAsync(unitOfWork, model);

            Assert.True(result);
        }

        [Fact]
        public async Task WithCorrectModel_ShouldAddNewCommentToPost()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost = new Post();
            var dbUser = new User();
            unitOfWork.Posts.Add(dbPost);
            unitOfWork.Users.Add(dbUser);
            unitOfWork.Complete();

            var model = this.CreateCorrectCommentModel(dbPost.Id);

            await this.CallAddCommentToPostAsync(unitOfWork, model);
            var dbPostFirstComments = dbPost.Comments.First();

            Assert.Equal(model.SourceId, dbPostFirstComments.PostId);
        }

        private CommentBindingModel CreateCorrectCommentModel(string postId)
        {
            var model = new CommentBindingModel()
            {
                SourceId = postId,
                Description = Guid.NewGuid().ToString()
            };

            return model;
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
