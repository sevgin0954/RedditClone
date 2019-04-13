using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.SortStrategies.PostStrategies.Interfaces
{
    public interface ISortPostsStrategy
    {
        Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId);

        Task<IEnumerable<Post>> GetSortedPostsAsync();
    }
}
