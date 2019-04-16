using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.SubredditStrategies
{
    public class SortSubredditByOld : ISubredditSortStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortSubredditByOld(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Subreddit> GetSortedSubreddits()
        {
            var sortedSubreddits = this.unitOfWork.Subreddits
                .GetAllAsQueryable()
                .OrderBy(s => s.CreatedDate);

            return sortedSubreddits;
        }
    }
}
