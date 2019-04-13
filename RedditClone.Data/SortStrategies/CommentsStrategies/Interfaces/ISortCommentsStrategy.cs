using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces
{
    public interface ISortCommentsStrategy
    {
        Task<IEnumerable<Comment>> GetSortedCommentsAsync(string postId);
    }
}
