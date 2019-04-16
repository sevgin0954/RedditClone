using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetPostWithOrderedCommentsAsync : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithoutIncorrectPostId_ShouldReturnNull()
        {
            var randomPostId = Guid.NewGuid().ToString();

            var model = await this.CallGetPostWithOrderedCommentsAsync(randomPostId);

            Assert.Null(model);
        }

        [Fact]
        public async Task WithoutComments_ShouldReturnModelWithZeroCommentsCount()
        {
            var dbPost = new Post();

            var model = await this.CallGetPostWithOrderedCommentsAsyncWithPost(dbPost);

            Assert.Equal(0, model.CommentsCount);
        }

        [Fact]
        public async Task WithCommentsWithReplies_ShouldReturnModelWithCorrectCommentsCount()
        {
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbCommentReplie = new Comment();
            dbComment.Replies.Add(dbCommentReplie);
            dbPost.Comments.Add(dbComment);

            var model = await this.CallGetPostWithOrderedCommentsAsyncWithPost(dbPost);

            Assert.Equal(2, model.CommentsCount);
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsync(string postId)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var service = this.GetService(unitOfWork);

            var requestCookies = new Mock<IRequestCookieCollection>().Object;
            var model = await service.GetPostWithOrderedCommentsAsync(postId, requestCookies);

            return model;
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsyncWithPost(Post post)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);

            var requestCookies = new Mock<IRequestCookieCollection>().Object;
            var model = await service.GetPostWithOrderedCommentsAsync(post.Id, requestCookies);

            return model;
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsyncWithCookies(
            Post post,
            IRequestCookieCollection requestCookies, 
            IResponseCookies responseCookies)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);
            
            var model = await service.GetPostWithOrderedCommentsAsync(post.Id, requestCookies);

            return model;
        }
    }
}
