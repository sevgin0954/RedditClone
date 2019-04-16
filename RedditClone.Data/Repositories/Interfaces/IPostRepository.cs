using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> GetByIdWithIncludedAllProperties(string postId);
        Task<IEnumerable<Post>> GetBySubcribedUserSortedByAsync(string userId, ISortPostsStrategy sortPostsStrategy);
        Task<IEnumerable<Post>> GetAllSortedByAsync(ISortPostsStrategy sortPostsStrategy);
        Task<IEnumerable<Post>> GetBySubredditSortedBy(string subredditId, ISortPostsStrategy sortPostsStrategy);
        Task<IEnumerable<Post>> GetByKeyWordsSortedByAsync(string[] keyWords, ISortPostsStrategy sortPostsStrategy);
    }
}
