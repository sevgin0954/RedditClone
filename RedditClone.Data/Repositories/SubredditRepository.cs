using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.Repositories
{
    public class SubredditRepository : BaseRepository<Subreddit>, ISubredditRepository
    {
        public SubredditRepository(RedditCloneDbContext dbContext) 
            : base(dbContext) { }

        public async Task<IEnumerable<Subreddit>> GetBySubcribedUserIdAsync(string userId)
        {
            var subreddits = await this.RedditCloneDbContext.Subreddits
                .Include(s => s.SubscribedUsers)
                .Where(s => s.SubscribedUsers.Any(su => su.UserId == userId))
                .ToListAsync();

            return subreddits;
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
