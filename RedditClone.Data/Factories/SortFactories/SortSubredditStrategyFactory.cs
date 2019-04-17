using RedditClone.Common.Enums.SortTypes;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.SubredditStrategies;
using RedditClone.Data.SortStrategies.SubredditStrategies.Interfaces;
using System.ComponentModel;

namespace RedditClone.Data.Factories.SortFactories
{
    public static class SortSubredditStrategyFactory
    {
        public static ISubredditSortStrategy GetSubredditSortStrategy(
            IRedditCloneUnitOfWork unitOfWork,
            SubredditSortType postSortType)
        {
            switch (postSortType)
            {
                case SubredditSortType.New:
                    return new SortSubredditByNew(unitOfWork);
                case SubredditSortType.Old:
                    return new SortSubredditByOld(unitOfWork);
                case SubredditSortType.Top:
                    return new SortSubredditByTop(unitOfWork);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
