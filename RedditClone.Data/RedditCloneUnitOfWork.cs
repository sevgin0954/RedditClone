using RedditClone.Data.Interfaces;
using RedditClone.Data.Repositories;
using RedditClone.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Data
{
    public class RedditCloneUnitOfWork : IRedditCloneUnitOfWork
    {
        private readonly RedditCloneDbContext dbContext;

        public RedditCloneUnitOfWork(RedditCloneDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.Posts = new PostRepository(dbContext);
            this.Comments = new CommentRepository(dbContext);
            this.Users = new UserRepository(dbContext);
            this.Subreddits = new SubredditRepository(dbContext);
            this.VotePostRepository = new VotePostRepository(dbContext);
        }

        public IPostRepository Posts { get; private set; }

        public ICommentRepository Comments { get; private set; }

        public IUserRepository Users { get; private set; }

        public ISubredditRepository Subreddits { get; private set; }

        public IVotePostRepository VotePostRepository { get; private set; }

        public int Complete()
        {
            return this.dbContext.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }
    }
}
