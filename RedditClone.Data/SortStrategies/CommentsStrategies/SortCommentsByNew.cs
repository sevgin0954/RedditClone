using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.CommentsStrategies
{
    public class SortCommentsByNew : ISortCommentsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortCommentsByNew(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Comment> GetSortedComments()
        {
            var comments = this.unitOfWork.Comments
                .GetAllAsQueryable()
                .OrderByDescending(c => c.PostDate);

            return comments;
        }
    }
}
