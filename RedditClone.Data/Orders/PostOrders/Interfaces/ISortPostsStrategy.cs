using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Orders.PostOrders.Interfaces
{
    public interface ISortPostsStrategy
    {
        Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId);

        Task<IEnumerable<Post>> GetSortedPostsAsync();
    }
}
