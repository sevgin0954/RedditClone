using System.Collections.Generic;
using System.Threading.Tasks;
using RedditClone.Data.Interfaces;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.Orders.PostOrders
{
    public class SortPostsByNew : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByNew(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubcribedUserOrderedByNewAsync(userId);
            return dbPosts;
        }
    }
}
