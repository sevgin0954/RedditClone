using RedditClone.Data.Repositories.Generic;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(RedditCloneDbContext dbContext) 
            : base(dbContext) { }
    }
}
