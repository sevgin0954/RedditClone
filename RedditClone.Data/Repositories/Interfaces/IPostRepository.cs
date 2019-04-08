using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetWithSubredditByAuthorAsync(string userId);
        Task<IEnumerable<Post>> GetBySubcribedUserOrderedByNewAsync(string userId);
        Task<IEnumerable<Post>> GetOrderByNewAsync();
        Task<IEnumerable<Post>> GetBySubcribedUserOrderedByTopAsync(string userId, TimeSpan timeFrame);
        Task<IEnumerable<Post>> GetOrderedByTopAsync(TimeSpan timeFrame);
        Task<IEnumerable<Post>> GetBySubscribedUserOrderedByControversialAsync(string userId, TimeSpan timeFrame);
        Task<IEnumerable<Post>> GetOrderedByControversialAsync(TimeSpan timeFrame);
        Task<IEnumerable<Post>> GetBySubscribedUserOrderedByBestAsync(string userId);
        Task<IEnumerable<Post>> GetOrderedByBestAsync();
    }
}
