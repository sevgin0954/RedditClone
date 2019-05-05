using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.Tests.Common;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public class GetPostWithOrderedCommentsAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithIncorrectPostId_ShouldReturnNull()
        {
            var incorrectPostId = Guid.NewGuid().ToString();

            var model = await this.CallGetPostWithOrderedCommentsAsync(incorrectPostId);

            Assert.Null(model);
        }

        [Fact]
        public async Task WithoutComments_ShouldReturnModelWithEmptyCommentsCollection()
        {
            var dbPost = new Post();

            var model = await this.CallGetPostWithOrderedCommentsAsync(dbPost);

            Assert.Empty(model.Comments);
        }

        [Fact]
        public async Task WithCommentWithReply_ShouldReturnModelWithCommentAndReply()
        {
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbCommentReplie = new Comment();
            dbComment.Replies.Add(dbCommentReplie);
            dbPost.Comments.Add(dbComment);

            var model = await this.CallGetPostWithOrderedCommentsAsync(dbPost);
            var modelFirstComment = model.Comments.First();
            var firstCommentReply = modelFirstComment.Replies.First();

            Assert.Equal(dbComment.Id, modelFirstComment.Id);
            Assert.Equal(dbCommentReplie.Id, firstCommentReply.Id);
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsync(string postId)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();

            var model = await this.CallGetPostWithOrderedCommentsAsync(unitOfWork, postId);

            return model;
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsync(Post post)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();

            var model = await this.CallGetPostWithOrderedCommentsAsync(unitOfWork, post.Id);

            return model;
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsync(
            IRedditCloneUnitOfWork unitOfWork, string postId)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;
            var mockedRequestCookies = new Mock<IRequestCookieCollection>();
            var model = await service.GetPostWithOrderedCommentsAsync(claimsPrincipal, postId, mockedRequestCookies.Object);

            return model;
        }
    }
}
