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
    public class AddResponseToCommentAsyncTests : BaseAddCommentToPostAsyncTest
    {
        [Fact]
        public async Task WithModelWithIncorrectCommentId_ShouldReturnFalse()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var randomCommentId = Guid.NewGuid().ToString();
            var model = new CommentBindingModel()
            {
                SourceId = randomCommentId
            };

            var result = await this.CallAddResponseToCommentAsync(unitOfWork, model);

            Assert.False(result);
        }

        [Fact]
        public async Task WithCorrectModel_ShouldReturnTrue()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbComment = new Comment();
            var dbReply = new Comment();
            dbComment.Replies.Add(dbReply);
            unitOfWork.Comments.Add(dbComment);
            unitOfWork.Complete();

            var model = this.CreateCorrectCommentModel(dbComment.Id);

            var result = await this.CallAddResponseToCommentAsync(unitOfWork, model);

            Assert.True(result);
        }

        [Fact]
        public async Task WithCorrectModel_ShouldAddNewReplyToComment()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbComment = new Comment();
            unitOfWork.Comments.Add(dbComment);
            unitOfWork.Complete();

            var model = this.CreateCorrectCommentModel(dbComment.Id);

            await this.CallAddResponseToCommentAsync(unitOfWork, model);

            Assert.Equal(1, dbComment.Replies.Count);
        }

        private CommentBindingModel CreateCorrectCommentModel(string commentId)
        {
            var model = new CommentBindingModel()
            {
                SourceId = commentId,
                Description = Guid.NewGuid().ToString()
            };

            return model;
        }

        private async Task<bool> CallAddResponseToCommentAsync(IRedditCloneUnitOfWork unitOfWork, CommentBindingModel model)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;

            var result = await service.AddResponseToCommentAsync(claimsPrincipal, model);
            return result;
        }
    }
}
