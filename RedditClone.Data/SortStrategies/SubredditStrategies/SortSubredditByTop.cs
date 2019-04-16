using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces;
using RedditClone.Models;
using System.Linq;

namespace RedditClone.Data.SortStrategies.SubredditStrategies
{
    public class SortSubredditByTop : ISubredditSortStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortSubredditByTop(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Subreddit> GetSortedSubreddits()
        {
            var sortedSubreddits = this.unitOfWork.Subreddits
                .GetAllAsQueryable()
                .OrderByDescending(s => s.SubscribedUsers.Count);

            return sortedSubreddits;
        }
    }
}
