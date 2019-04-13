using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.SortStrategies.PostOrders
{
    public class SortPostsByBest : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByBest(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubscribedUserOrderedByBestAsync(userId);
            return dbPosts;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsAsync()
        {
            var dbPosts = await this.unitOfWork.Posts.GetOrderedByBestAsync();
            return dbPosts;
        }
    }
}
