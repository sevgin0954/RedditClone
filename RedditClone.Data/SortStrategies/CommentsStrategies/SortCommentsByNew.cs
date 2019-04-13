using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Comment>> GetSortedCommentsAsync(string postId)
        {
            var sortedComments = await unitOfWork.Comments.GetByPostOrderedByNewAsync(postId);
            return sortedComments;
        }
    }
}
