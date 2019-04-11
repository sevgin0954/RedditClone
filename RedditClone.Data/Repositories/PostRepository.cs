using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;
using System;
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

        public async Task<IEnumerable<Post>> GetBySubcribedUserOrderedByNewAsync(string userId)
        {
            var postsQueryable = this.GetOrderedByNew();
            var filteredPosts = await postsQueryable
                .Where(p => p.Subreddit.SubscribedUsers.Any(su => su.UserId == userId))
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<Post>> GetOrderByNewAsync()
        {
            var postsQueryable = this.GetOrderedByNew();
            var posts = await postsQueryable.ToListAsync();

            return posts;
        }

        private IQueryable<Post> GetOrderedByNew()
        {
            var postsQueryable = this.RedditCloneDbContext.Posts
                .Include(p => p.Subreddit)
                .Include(p => p.Author)
                .OrderByDescending(p => p.PostDate);

            return postsQueryable;
        }

        public async Task<IEnumerable<Post>> GetBySubcribedUserOrderedByTopAsync(string userId, TimeSpan timeFrame)
        {
            var postsQueryable = this.GetOrderedByTop(timeFrame);
            var filteredPosts = await postsQueryable
                .Where(p => p.Subreddit.SubscribedUsers.Any(su => su.UserId == userId))
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<Post>> GetOrderedByTopAsync(TimeSpan timeFrame)
        {
            var postsQueryable = this.GetOrderedByTop(timeFrame);
            var posts = await postsQueryable.ToListAsync();

            return posts;
        }

        private IQueryable<Post> GetOrderedByTop(TimeSpan timeFrame)
        {
            var startDate = DateTime.UtcNow - timeFrame;

            var postsQueryable = this.RedditCloneDbContext.Posts
                   .Where(p => p.PostDate >= startDate)
                   .Include(p => p.Subreddit)
                   .Include(p => p.Author)
                   .OrderByDescending(p => p.UpVotesCount)
                   .ThenByDescending(p => p.PostDate);

            return postsQueryable;
        }

        public async Task<IEnumerable<Post>> GetBySubscribedUserOrderedByControversialAsync(string userId, TimeSpan timeFrame)
        {
            var postsQueryable = this.GetOrderedByControversial(timeFrame);
            var filteredPosts = await postsQueryable
                .Where(s => s.Subreddit.SubscribedUsers.Any(su => su.UserId == userId))
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<Post>> GetOrderedByControversialAsync(TimeSpan timeFrame)
        {
            var postsQueryable = this.GetOrderedByControversial(timeFrame);
            var posts = await postsQueryable.ToListAsync();

            return posts;
        }

        private IQueryable<Post> GetOrderedByControversial(TimeSpan timeFrame)
        {
            var startDate = DateTime.UtcNow - timeFrame;

            var postsQueryable = this.RedditCloneDbContext.Posts
                .Where(p => p.PostDate >= startDate)
                .OrderByDescending(p => p.UpVotesCount + p.DownVotesCount)
                .ThenByDescending(p => p.PostDate)
                .Include(p => p.Subreddit)
                .Include(p => p.Author);

            return postsQueryable;
        }

        public async Task<IEnumerable<Post>> GetBySubscribedUserOrderedByBestAsync(string userId)
        {
            var postsQueryable = this.GetOrderedByBest();
            var filteredPosts = await postsQueryable
                .Where(s => s.Subreddit.SubscribedUsers.Any(su => su.UserId == userId))
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<Post>> GetOrderedByBestAsync()
        {
            var postsQueryable = this.GetOrderedByBest();
            var posts = await postsQueryable.ToListAsync();

            return posts;
        }

        private IQueryable<Post> GetOrderedByBest()
        {
            var startDate = DateTime.UtcNow - TimeSpan.FromDays(1);

            var postsQueryable = this.RedditCloneDbContext.Posts
                .Where(p => p.PostDate >= startDate)
                .Include(p => p.Author)
                .Include(p => p.Subreddit)
                .OrderByDescending(p => p.UpVotesCount - p.DownVotesCount)
                .ThenByDescending(p => p.PostDate);

            return postsQueryable;
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
