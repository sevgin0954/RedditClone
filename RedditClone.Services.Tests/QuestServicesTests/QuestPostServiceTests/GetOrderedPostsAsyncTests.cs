using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetOrderedPostsAsyncTests : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithoutPosts_ShouldReturnModelWithZeroPosts()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();

            var model = await this.CallGetOrderedPostsAsync(unitOfWork);

            Assert.Empty(model.Posts);
        }

        [Fact]
        public async Task WithPosts_ShouldReturnModelWithAllPosts()
        {
            var dbPost1 = this.CreatePostWithCurrentTime();
            var dbPost2 = this.CreatePostWithCurrentTime();

            var model = await this.CallGetOrderedPostsAsyncWithPosts(dbPost1, dbPost2);

            var modelPosts = model.Posts;
            var modelPost1 = modelPosts.ElementAt(0);
            var modelPost2 = modelPosts.ElementAt(1);

            Assert.Equal(2, modelPosts.Count());
            Assert.Contains(modelPosts, p => p.Id == modelPost1.Id);
            Assert.Contains(modelPosts, p => p.Id == modelPost2.Id);
        }

        private Post CreatePostWithCurrentTime()
        {
            var dbPost = new Post()
            {
                PostDate = DateTime.UtcNow
            };

            return dbPost;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithPosts(params Post[] posts)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();

            var model = await this.CallGetOrderedPostsAsync(unitOfWork);
            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithCookies(
            IRequestCookieCollection requestCokieCollection,
            IResponseCookies responseCookies)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var service = this.GetService(unitOfWork);
            var model = await service.GetOrderedPostsAsync(requestCokieCollection, responseCookies);

            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsync(IRedditCloneUnitOfWork unitOfWork)
        {
            var service = this.GetService(unitOfWork);

            var requestCookieCollection = new Mock<IRequestCookieCollection>().Object;
            var responseCookies = new Mock<IResponseCookies>().Object;

            var model = await service.GetOrderedPostsAsync(requestCookieCollection, responseCookies);
            return model;
        }
    }
}
