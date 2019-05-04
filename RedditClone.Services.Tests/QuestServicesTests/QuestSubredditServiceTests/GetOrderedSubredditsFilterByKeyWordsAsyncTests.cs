using RedditClone.Common.Enums.SortTypes;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestSubredditServiceTests
{
    public class GetOrderedSubredditsFilterByKeyWordsAsyncTests : BaseQuestSubredditServiceTest
    {
        [Fact]
        public async Task WithKeyWord_ShouldReturnFilteredSubreddits()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbSubreddit1 = this.CreateSubreddit("as sad s");
            var dbSubreddit2 = this.CreateSubreddit("New game");
            this.AddSubredditsToUnitOfWork(unitOfWork, dbSubreddit1, dbSubreddit2);

            var service = this.GetService(unitOfWork);

            var keyWords = new string[] { "game" };
            var model = await service.GetOrderedSubredditsFilterByKeyWordsAsync(keyWords, SubredditSortType.New);
            var modelSubreddits = model.Subreddits;

            Assert.Single(modelSubreddits);
            Assert.Equal(dbSubreddit2.Id, modelSubreddits.First().Id);
        }

        private Subreddit CreateSubreddit(string name)
        {
            var subreddit = new Subreddit()
            {
                Name = name
            };

            return subreddit;
        }

        private void AddSubredditsToUnitOfWork(IRedditCloneUnitOfWork unitOfWork, params Subreddit[] subreddits)
        {
            unitOfWork.Subreddits.AddRange(subreddits);
            unitOfWork.Complete();
        }
    }
}
