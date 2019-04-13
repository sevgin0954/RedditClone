using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Comment>> GetSortedCommentsAsync(string postId)
        {
            var sortedComments = await unitOfWork.Comments.GetByPostOrderedByTopAsync(postId);
            return sortedComments;
        }
    }
}
