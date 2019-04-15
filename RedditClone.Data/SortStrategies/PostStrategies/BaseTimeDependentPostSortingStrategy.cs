using System;
using System.Linq;
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

        public abstract IQueryable<Post> GetSortedPosts();
    }
}
