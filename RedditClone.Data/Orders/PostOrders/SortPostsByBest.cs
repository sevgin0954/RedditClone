using RedditClone.Data.Interfaces;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Orders.PostOrders
{
    public class SortPostsByBest : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByBest(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubscribedUserOrderedByBestAsync(userId);
            return dbPosts;
        }
    }
}
