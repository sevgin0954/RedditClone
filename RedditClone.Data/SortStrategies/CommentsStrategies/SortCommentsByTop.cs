using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.CommentsStrategies
{
    public class SortCommentsByTop : ISortCommentsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortCommentsByTop(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Comment> GetSortedComments()
        {
            var comments = this.unitOfWork.Comments
                .GetAllAsQueryable()
                .OrderByDescending(p => p.UpVotesCount)
                   .ThenByDescending(p => p.PostDate);

            return comments;
        }
    }
}
