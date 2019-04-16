using RedditClone.Models;
using System.Linq;

namespace RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces
{
    public interface ISubredditSortStrategy
    {
        IQueryable<Subreddit> GetSortedSubreddits();
    }
}
