using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Orders.PostOrders.Interfaces
{
    public interface ISortPostsStrategy
    {
        Task<IEnumerable<Post>> GetSortedPostsAsync(string userId);
    }
}
