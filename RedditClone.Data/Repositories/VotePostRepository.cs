using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories
{
    class VotePostRepository : BaseRepository<VotePost>, IVotePostRepository
    {
        public VotePostRepository(DbContext dbContext)
            : base(dbContext) { }

        public async Task<VotePost> GetByUserIdAsync(string userId, string postId)
        {
            var votePost = await this.RedditCloneDbContext.VotesPosts
                .Where(v => v.UserId == userId && v.PostId == postId)
                .FirstOrDefaultAsync();

            return votePost;
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
