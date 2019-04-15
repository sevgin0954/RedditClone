using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
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

        public async Task<IEnumerable<Comment>> GetByPostSortedByAsync(string postId, ISortCommentsStrategy sortStrategy)
        {
            var sortedComments = sortStrategy.GetSortedComments();
            var filteredComments = await sortedComments
                .Where(c => c.PostId == postId)
                .Include(c => c.Author)
                .Include(c => c.Replies)
                .ToListAsync();

            return filteredComments;
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
