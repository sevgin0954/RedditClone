using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.Orders.PostOrders
{
    public abstract class BaseTimeDependentPostSortingStrategy : ISortPostsStrategy
    {
        protected BaseTimeDependentPostSortingStrategy(TimeSpan timeFrame)
        {
            TimeFrame = timeFrame;
        }

        protected TimeSpan TimeFrame { get; }

        public abstract Task<IEnumerable<Post>> GetSortedPostsAsync(string userId);
    }
}
