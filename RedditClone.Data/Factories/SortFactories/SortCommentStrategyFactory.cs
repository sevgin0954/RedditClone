using RedditClone.Common.Enums;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using System.ComponentModel;

namespace RedditClone.Data.Factories.SortFactories
{
    public static class SortCommentStrategyFactory
    {
        public static ISortCommentsStrategy GetSortPostsStrategy(
            IRedditCloneUnitOfWork unitOfWork, SortType sortType)
        {
            switch (sortType)
            {
                case SortType.New:
                    return new SortCommentsByNew(unitOfWork);
                case SortType.Top:
                    return new SortCommentsByTop(unitOfWork);
                case SortType.Controversial:
                    return new SortCommentsByControversial(unitOfWork);
                case SortType.Best:
                    return new SortCommentsByBest(unitOfWork);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
