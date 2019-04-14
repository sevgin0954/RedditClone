using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Services.Tests.Common;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public class GetOrderedPostsAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithUserWithSubscribedSubreddits_ShouldReturnModelWithOnlySubsribedSubredditsPosts()
        {
            var dbUser = new User();
            var dbPost1 = this.CreatePostWithCurrentTime();
            var dbPost2 = this.CreatePostWithCurrentTime();
            var dbPost3 = this.CreatePostWithCurrentTime();
            var dbSubreddit = new Subreddit();
            dbSubreddit.Posts.Add(dbPost1);
            dbSubreddit.Posts.Add(dbPost2);
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);
                
            var model = await this.CallGetOrderedPostsAsyncWithUserAndPosts(dbUser, dbPost3);

            var modelPosts = model.Posts;
            var modelPost1 = modelPosts.ElementAt(0);
            var modelPost2 = modelPosts.ElementAt(1);

            Assert.Equal(2, modelPosts.Count());
            Assert.Contains(modelPosts, p => p.Id == dbPost1.Id);
            Assert.Contains(modelPosts, p => p.Id == dbPost2.Id);
        }

        [Fact]
        public async Task WithUserWithoudSubscribedSubreddits_ShouldReturnModelWithAllPosts()
        {
            var dbUser = new User();
            var dbPost1 = this.CreatePostWithCurrentTime();
            var dbPost2 = this.CreatePostWithCurrentTime();

            var model = await this.CallGetOrderedPostsAsyncWithUserAndPosts(dbUser, dbPost1, dbPost2);

            var modelPosts = model.Posts;
            var modelPost1 = modelPosts.ElementAt(0);
            var modelPost2 = modelPosts.ElementAt(1);

            Assert.Equal(2, modelPosts.Count());
            Assert.Contains(modelPosts, p => p.Id == dbPost1.Id);
            Assert.Contains(modelPosts, p => p.Id == dbPost2.Id);
        }

        private Post CreatePostWithCurrentTime()
        {
            var dbPost = new Post()
            {
                PostDate = DateTime.UtcNow
            };

            return dbPost;
        }

        private void SubscribeUserToSubreddit(User user, Subreddit subreddit)
        {
            var dbUserSubreddit = new UserSubreddit()
            {
                Subreddit = subreddit
            };
            user.SubscribedSubreddits.Add(dbUserSubreddit);
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithCookies(
            IRequestCookieCollection requestCokieCollection, 
            IResponseCookies responseCookies)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var userManager = CommonTestMethods.GetMockedUserManager().Object;

            var service = this.GetService(unitOfWork, userManager);

            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;
            var model = await service.GetOrderedPostsAsync(
                claimsPrincipal,
                requestCokieCollection,
                responseCookies);

            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithPosts(params Post[] posts)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();

            var userManager = CommonTestMethods.GetMockedUserManager().Object;
            var service = this.GetService(unitOfWork, userManager);

            var claimsPricipal = new Mock<ClaimsPrincipal>().Object;
            var requestCookieCollection = new Mock<IRequestCookieCollection>().Object;
            var responseCookies = new Mock<IResponseCookies>().Object;
            var model = await service.GetOrderedPostsAsync(
                claimsPricipal,
                requestCookieCollection,
                responseCookies);

            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithUser(User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var model = await this.CallGetOrderedPostsAsyncWithUser(unitOfWork, user);

            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithUserAndPosts(User user, params Post[] posts)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();

            var model = await this.CallGetOrderedPostsAsyncWithUser(unitOfWork, user);

            return model;
        }

        private async Task<IndexViewModel> CallGetOrderedPostsAsyncWithUser(IRedditCloneUnitOfWork unitOfWork, User user)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            mockedUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(user.Id);

            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            var claimsPricipal = new Mock<ClaimsPrincipal>().Object;
            var requestCookieCollection = new Mock<IRequestCookieCollection>().Object;
            var responseCookies = new Mock<IResponseCookies>().Object;
            var model = await service.GetOrderedPostsAsync(
                claimsPricipal,
                requestCookieCollection,
                responseCookies);

            return model;
        }
    }
}
