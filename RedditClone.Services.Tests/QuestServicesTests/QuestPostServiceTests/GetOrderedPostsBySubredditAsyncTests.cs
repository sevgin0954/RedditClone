using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetOrderedPostsBySubredditAsyncTests : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithIncorrectSubredditId_ShouldReturnNull()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();

            var incorrectSubredditId = Guid.NewGuid().ToString();
            var model = await this.CallGetOrderedPostsBySubredditAsync(unitOfWork, incorrectSubredditId);

            Assert.Null(model);
        }

        [Fact]
        public async Task WithPostsWithSubredditsAndSubredditId_ShouldReturnModelWithPostsOnlyFromSubreddit()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbSubreddit1 = this.CreateSubredditWithPost();
            var dbSubreddit2 = this.CreateSubredditWithPost();
            unitOfWork.Subreddits.AddRange(dbSubreddit1, dbSubreddit2);
            unitOfWork.Complete();

            var subredditId = dbSubreddit1.Id;
            var model = await this.CallGetOrderedPostsBySubredditAsync(unitOfWork, subredditId);

            Assert.Single(model.Posts);
        }

        private Subreddit CreateSubredditWithPost()
        {
            var subreddit = new Subreddit();
            this.AddPostWithCurrectTimeToSubreddit(subreddit);

            return subreddit;
        }

        private void AddPostWithCurrectTimeToSubreddit(Subreddit subreddit)
        {
            var dbPost = new Post()
            {
                PostDate = DateTime.UtcNow
            };
            subreddit.Posts.Add(dbPost);
        }

        private async Task<PostsViewModel> CallGetOrderedPostsBySubredditAsync(
            IRedditCloneUnitOfWork unitOfWork, 
            string subredditId)
        {
            var requestCookies = new Mock<IRequestCookieCollection>().Object;

            var service = this.GetService(unitOfWork);
            var model = await service.GetOrderedPostsBySubredditAsync(subredditId, requestCookies);

            return model;
        }
    }
}
