using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.SubredditStrategies
{
    class SortSubredditByNew : ISubredditSortStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortSubredditByNew(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Subreddit> GetSortedSubreddits()
        {
            var sortedSubreddits = this.unitOfWork.Subreddits
                .GetAllAsQueryable()
                .OrderByDescending(s => s.CreatedDate);

            return sortedSubreddits;
        }
    }
}
