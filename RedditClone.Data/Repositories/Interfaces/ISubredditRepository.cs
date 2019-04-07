using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface ISubredditRepository : IRepository<Subreddit>
    {
        Task<IEnumerable<Subreddit>> GetBySubcribedUserAsync(string userId);

        Task<IEnumerable<Subreddit>> GetByAuthorAsync(string authorId);
    }
}
