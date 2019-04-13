using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(RedditCloneDbContext dbContext)
            : base(dbContext) { }

        public async Task<IEnumerable<Comment>> GetByPostOrderedByNewAsync(string postId)
        {
            var comments = await this.RedditCloneDbContext.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.PostDate)
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<Comment>> GetByPostOrderedByTopAsync(string postId)
        {
            var comments = await this.RedditCloneDbContext.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(p => p.UpVotesCount)
                   .ThenByDescending(p => p.PostDate)
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<Comment>> GetByPostOrderedByControversialAsync(string postId)
        {
            var comments = await this.RedditCloneDbContext.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(p => p.UpVotesCount + p.DownVotesCount)
                .ThenByDescending(p => p.PostDate)
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<Comment>> GetByPostOrderedByBestAsync(string postId)
        {
            var comments = await this.RedditCloneDbContext.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(p => p.UpVotesCount - p.DownVotesCount)
                .ThenByDescending(p => p.PostDate)
                .Include(p => p.Author)
                .Include(p => p.Replies)
                .ToListAsync();

            return comments;
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
