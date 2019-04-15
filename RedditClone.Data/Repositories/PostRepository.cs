using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
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

        public async Task<Post> GetByIdWithIncludedAllProperties(string postId)
        {
            var post = await this.RedditCloneDbContext.Posts
                .Where(p => p.Id == postId)
                .Include(p => p.Author)
                .Include(p => p.Subreddit)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync();

            return post;
        }

        public async Task<IEnumerable<Post>> GetBySubcribedUserSortedByAsync(string userId, ISortPostsStrategy sortPostsStrategy)
        {
            var sortedPosts = await sortPostsStrategy
                .GetSortedPosts()
                .Where(p => p.Subreddit.SubscribedUsers.Any(su => su.UserId == userId))
                .Include(p => p.Subreddit)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ToListAsync();

            return sortedPosts;
        }

        public async Task<IEnumerable<Post>> GetAllSortedByAsync(ISortPostsStrategy sortPostsStrategy)
        {
            var sortedPosts = await sortPostsStrategy
                .GetSortedPosts()
                .Include(p => p.Subreddit)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ToListAsync();

            return sortedPosts;
        }

        public async Task<IEnumerable<Post>> GetBySubredditSortedBy(string subredditId, ISortPostsStrategy sortPostsStrategy)
        {
            var sortedPosts = await sortPostsStrategy
                .GetSortedPosts()
                .Where(p => p.SubredditId == subredditId)
                .Include(p => p.Subreddit)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ToListAsync();

            return sortedPosts;
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
