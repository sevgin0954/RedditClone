using RedditClone.Common.Enums;
using RedditClone.Data.Interfaces;
using RedditClone.Data.Orders.PostOrders;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using System;
using System.ComponentModel;

namespace RedditClone.Data.Factories.PostsFactories
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