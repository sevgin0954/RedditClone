using RedditClone.Common.Enums;
using RedditClone.Data.Interfaces;
using RedditClone.Data.Orders.PostOrders;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using System;
using System.ComponentModel;

namespace RedditClone.Data.Factories.PostsFactories
{
    public static class SortPostsFactory
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
