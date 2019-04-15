using RedditClone.Models;
using System.Linq;

namespace RedditClone.Data.SortStrategies.PostStrategies.Interfaces
{
    public interface ISortPostsStrategy
    {
        IQueryable<Post> GetSortedPosts();
    }
}
