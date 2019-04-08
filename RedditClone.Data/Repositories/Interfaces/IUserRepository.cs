using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Models;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetWithPostsWithSubredditAndCommentsAsync(string userId);
    }
}
