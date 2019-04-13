using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.PostOrders
{
    public abstract class BaseTimeDependentPostSortingStrategy : ISortPostsStrategy
    {
        protected BaseTimeDependentPostSortingStrategy(TimeSpan timeFrame)
        {
            TimeFrame = timeFrame;
        }

        protected TimeSpan TimeFrame { get; }

        public abstract Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId);

        public abstract Task<IEnumerable<Post>> GetSortedPostsAsync();
    }
}
