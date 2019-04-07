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

        public async Task<IEnumerable<Comment>> GetWithPostByUserIdAsync(string userId)
        {
            var comments = await this.RedditCloneDbContext.Comments
                .Include(c => c.Post)
                .Where(c => c.AuthorId == userId)
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
