using RedditClone.Common.Enums.SortTypes;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using System.ComponentModel;

namespace RedditClone.Data.Factories.SortFactories
{
    public static class SortCommentStrategyFactory
    {
        public static ISortCommentsStrategy GetSortPostsStrategy(
            IRedditCloneUnitOfWork unitOfWork, CommentSortType sortType)
        {
            switch (sortType)
            {
                case CommentSortType.New:
                    return new SortCommentsByNew(unitOfWork);
                case CommentSortType.Old:
                    return new SortCommentsByOld(unitOfWork);
                case CommentSortType.Top:
                    return new SortCommentsByTop(unitOfWork);
                case CommentSortType.Controversial:
                    return new SortCommentsByControversial(unitOfWork);
                case CommentSortType.Best:
                    return new SortCommentsByBest(unitOfWork);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
