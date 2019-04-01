using RedditClone.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Data.Interfaces
{
    public interface IRedditCloneUnitOfWork
    {
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        IUserRepository Users { get; }
        ISubredditRepository Subreddits { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
