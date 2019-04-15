using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostSortedByAsync(string postId, ISortCommentsStrategy sortStrategy);
    }
}
