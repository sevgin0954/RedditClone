using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(RedditCloneDbContext dbContext)
            : base(dbContext) { }

        public async Task<IEnumerable<Post>> GetAllWithSubredditByUserIdAsync(string userId)
        {
            var posts = await this.RedditCloneDbContext.Posts
                .Include(p => p.Subreddit)
                .Where(p => p.AuthorId == userId)
                .ToListAsync();

            return posts;
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
