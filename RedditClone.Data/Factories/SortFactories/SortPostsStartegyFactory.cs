using RedditClone.Common.Enums;
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
            SortType postSortType)
        {
            switch (postSortType)
            {
                case SortType.New:
                    return new SortPostsByNew(unitOfWork);
                case SortType.Top:
                    return new SortPostsByTop(unitOfWork, timeFrame);
                case SortType.Controversial:
                    return new SortPostsByControversial(unitOfWork, timeFrame);
                case SortType.Best:
                    return new SortPostsByBest(unitOfWork);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}