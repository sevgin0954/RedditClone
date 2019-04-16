using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.SubredditStrategies
{
    public class SortSubredditByPosts : ISubredditSortStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortSubredditByPosts(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Subreddit> GetSortedSubreddits()
        {
            var sortedSubreddits = this.unitOfWork.Subreddits
                .GetAllAsQueryable()
                .OrderByDescending(s => s.Posts.Count);

            return sortedSubreddits;
        }
    }
}
