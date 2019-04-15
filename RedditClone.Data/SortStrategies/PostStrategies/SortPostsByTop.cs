using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostOrders;
using RedditClone.Models;
using System;
using System.Linq;

namespace RedditClone.Data.SortStrategies.PostStrategies
{
    public class SortPostsByTop : BaseTimeDependentPostSortingStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByTop(IRedditCloneUnitOfWork unitOfWork, TimeSpan timeFrame)
            : base(timeFrame)
        {
            this.unitOfWork = unitOfWork;
        }

        public override IQueryable<Post> GetSortedPosts()
        {
            var startDate = DateTime.UtcNow.Subtract(this.TimeFrame);

            var postsQueryable = this.unitOfWork.Posts
                   .GetAllAsQueryable()
                   .Where(p => p.PostDate >= startDate)
                   .OrderByDescending(p => p.UpVotesCount)
                   .ThenByDescending(p => p.PostDate);

            return postsQueryable;
        }
    }
}
