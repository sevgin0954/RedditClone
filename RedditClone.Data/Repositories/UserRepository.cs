using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(RedditCloneDbContext dbContext) 
            : base(dbContext) { }

        public async Task<User> GetWithPostsWithSubredditAndCommentsAsync(string userId)
        {
            var user = await this.RedditCloneDbContext.Users
                .Include(u => u.Posts)
                    .ThenInclude(c => c.Subreddit)
                .Include(u => u.Comments)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            return user;
        }

        public RedditCloneDbContext RedditCloneDbContext
        {
            get
            {
                return this.DbContext as RedditCloneDbContext;
            }
        }
    }
}
