using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Models;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface IVotePostRepository : IRepository<VotePost>
    {
        Task<VotePost> GetByUserIdAsync(string userId, string postId);
    }
}
