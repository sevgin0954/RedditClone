using System.Linq;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.PostStrategies
{
    public class SortPostsByNew : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByNew(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Post> GetSortedPosts()
        {
            var postsQueryable =  this.unitOfWork.Posts
                .GetAllAsQueryable()
                .OrderByDescending(p => p.PostDate);

            return postsQueryable;
        }
    }
}
