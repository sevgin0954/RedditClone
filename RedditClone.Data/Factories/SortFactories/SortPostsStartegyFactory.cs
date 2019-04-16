using RedditClone.Common.Enums.SortTypes;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostOrders;
using RedditClone.Data.SortStrategies.PostStrategies;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using System;
using System.ComponentModel;

namespace RedditClone.Data.Factories.SortFactories
{
    public static class SortPostsStartegyFactory
    {
        public static ISortPostsStrategy GetSortPostsStrategy(
            IRedditCloneUnitOfWork unitOfWork,
            TimeSpan timeFrame,
            PostSortType postSortType)
        {
            switch (postSortType)
            {
                case PostSortType.New:
                    return new SortPostsByNew(unitOfWork);
                case PostSortType.Top:
                    return new SortPostsByTop(unitOfWork, timeFrame);
                case PostSortType.Controversial:
                    return new SortPostsByControversial(unitOfWork, timeFrame);
                case PostSortType.Best:
                    return new SortPostsByBest(unitOfWork);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}