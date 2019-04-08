using RedditClone.Data.Interfaces;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using RedditClone.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Orders.PostOrders
{
    public class SortPostsByControversial : BaseTimeDependentPostSortingStrategy, ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByControversial(IRedditCloneUnitOfWork unitOfWork, TimeSpan timeFrame)
            : base(timeFrame)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubscribedUserOrderedByControversialAsync(userId, this.TimeFrame);
            return dbPosts;
        }

        public override async Task<IEnumerable<Post>> GetSortedPostsAsync()
        {
            var dbPosts = await this.unitOfWork.Posts.GetOrderedByControversialAsync(this.TimeFrame);
            return dbPosts;
        }
    }
}
