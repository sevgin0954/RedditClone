using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Tests.Common;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.UserServicesTests.UserPostServiceTests
{
    public class GetOrderedPostsAsyncTests : BaseUserPostServiceTest
    {
        [Fact]
        public async Task WithoutCookies_ShouldSetDefaultCookies()
        {
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();
            var mockedResponseCookies = new Mock<IResponseCookies>();

            await this.CallOrderedPostsAsyncWithCookies(mockedRequestCookieCollection.Object, mockedResponseCookies.Object);

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = WebConstants.CookieDefaultValuePostSortType;
            mockedResponseCookies
                .Verify(rc => rc.Append(postSortTypeKey, postSortTypeValue, It.IsAny<CookieOptions>()), Times.Once);

            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;
            mockedResponseCookies
                .Verify(rc => rc.Append(postTimeFrameKey, postTimeFrameValue, It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        [Fact]
        public async Task WithNotTimeDependentSortType_ShouldReturnModelWithNullPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.New.ToString());

            var model = await this.CallOrderedPostsAsyncWithCookies(
                mockedRequestCookieCollection.Object, 
                mockedResponseCookies.Object);

            Assert.Null(model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithTimeDependentSortType_ShouldReturnModelWithPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.Controversial.ToString());

            var model = await this.CallOrderedPostsAsyncWithCookies(
                mockedRequestCookieCollection.Object,
                mockedResponseCookies.Object);

            Assert.NotNull(model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithTimeDependentSortTypeAndPostShowTimeFrameCookie_ShouldReturnModelWithCorrectPostShowTimeFrame()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.Controversial.ToString());

            var postShowTimeFrame = PostShowTimeFrame.PastMonth;
            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            mockedRequestCookieCollection.SetupGet(r => r[postTimeFrameKey])
                .Returns(postShowTimeFrame.ToString());

            var model = await this.CallOrderedPostsAsyncWithCookies(
                mockedRequestCookieCollection.Object,
                mockedResponseCookies.Object);

            Assert.Equal(postShowTimeFrame, model.PostShowTimeFrame);
        }

        [Fact]
        public async Task WithNotTimeDependentSortType_ShouldSetDefaultPostShowTimeFrameCookie()
        {
            var mockedResponseCookies = new Mock<IResponseCookies>();
            var mockedRequestCookieCollection = new Mock<IRequestCookieCollection>();

            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            mockedRequestCookieCollection.SetupGet(r => r[postSortTypeKey])
                .Returns(PostSortType.New.ToString());

            var model = await this.CallOrderedPostsAsyncWithCookies(
                mockedRequestCookieCollection.Object, 
                mockedResponseCookies.Object);

            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;
            mockedResponseCookies
                .Verify(rc => rc.Append(postTimeFrameKey, postTimeFrameValue, It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        [Fact]
        public async Task WithQuest_ShouldReturnModelWithAllPosts()
        {
            var dbPost1 = this.CreatePostWithCurrectTime();
            var dbPost2 = this.CreatePostWithCurrectTime();

            var model = await this.CallOrderedPostsAsyncWithPosts(dbPost1, dbPost2);

            var modelPosts = model.Posts;
            var modelPost1 = modelPosts.ElementAt(0);
            var modelPost2 = modelPosts.ElementAt(1);

            Assert.Equal(2, modelPosts.Count());
            Assert.Contains(modelPosts, p => p.Id == modelPost1.Id);
            Assert.Contains(modelPosts, p => p.Id == modelPost2.Id);

        }

        [Fact]
        public async Task WithUserWithSubscribedSubreddits_ShouldReturnModelWithOnlySubsribedSubredditsPosts()
        {
            var dbUser = new User();
            var dbPost1 = this.CreatePostWithCurrectTime();
            var dbPost2 = this.CreatePostWithCurrectTime();
            var dbPost3 = this.CreatePostWithCurrectTime();
            var dbSubreddit = new Subreddit();
            dbSubreddit.Posts.Add(dbPost1);
            dbSubreddit.Posts.Add(dbPost2);
            this.SubscribeUserToSubreddit(dbUser, dbSubreddit);
                
            var model = await this.CallOrderedPostsAsyncWithUserAndPosts(dbUser, dbPost3);

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
            var dbPost1 = this.CreatePostWithCurrectTime();
            var dbPost2 = this.CreatePostWithCurrectTime();

            var model = await this.CallOrderedPostsAsyncWithUserAndPosts(dbUser, dbPost1, dbPost2);

            var modelPosts = model.Posts;
            var modelPost1 = modelPosts.ElementAt(0);
            var modelPost2 = modelPosts.ElementAt(1);

            Assert.Equal(2, modelPosts.Count());
            Assert.Contains(modelPosts, p => p.Id == dbPost1.Id);
            Assert.Contains(modelPosts, p => p.Id == dbPost2.Id);
        }

        private Post CreatePostWithCurrectTime()
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

        private async Task<IndexViewModel> CallOrderedPostsAsyncWithCookies(
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

        private async Task<IndexViewModel> CallOrderedPostsAsyncWithPosts(params Post[] posts)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();

            var userManager = CommonTestMethods.GetMockedUserManager().Object;
            var signInManager = CommonTestMethods.GetMockedSignInManager(userManager).Object;
            var service = this.GetService(unitOfWork, userManager, signInManager);

            var claimsPricipal = new Mock<ClaimsPrincipal>().Object;
            var requestCookieCollection = new Mock<IRequestCookieCollection>().Object;
            var responseCookies = new Mock<IResponseCookies>().Object;
            var model = await service.GetOrderedPostsAsync(
                claimsPricipal,
                requestCookieCollection,
                responseCookies);

            return model;
        }

        private async Task<IndexViewModel> CallOrderedPostsAsyncWithUser(User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var model = await this.CallOrderedPostsAsyncWithUser(unitOfWork, user);

            return model;
        }

        private async Task<IndexViewModel> CallOrderedPostsAsyncWithUserAndPosts(User user, params Post[] posts)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();

            var model = await this.CallOrderedPostsAsyncWithUser(unitOfWork, user);

            return model;
        }

        private async Task<IndexViewModel> CallOrderedPostsAsyncWithUser(IRedditCloneUnitOfWork unitOfWork, User user)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            mockedUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(user.Id);

            var mockedSignInManager = CommonTestMethods.GetMockedSignInManager(mockedUserManager.Object);
            mockedSignInManager.Setup(sim => sim.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns(true);

            var service = this.GetService(unitOfWork, mockedUserManager.Object, mockedSignInManager.Object);

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
