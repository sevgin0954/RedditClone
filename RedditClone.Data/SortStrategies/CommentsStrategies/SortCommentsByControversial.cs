using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.CommentsStrategies
{
    public class SortCommentsByControversial : ISortCommentsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortCommentsByControversial(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Comment> GetSortedComments()
        {
            var comments = this.unitOfWork.Comments
                .GetAllAsQueryable()
                .OrderByDescending(p => p.UpVotesCount + p.DownVotesCount)
                .ThenByDescending(p => p.PostDate);

            return comments;
        }
    }
}
