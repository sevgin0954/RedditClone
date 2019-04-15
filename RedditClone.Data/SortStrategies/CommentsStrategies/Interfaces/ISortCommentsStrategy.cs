using RedditClone.Models;
using System.Linq;

namespace RedditClone.Data.SortStrategies.CommentsStrategies.Interfaces
{
    public interface ISortCommentsStrategy
    {
        IQueryable<Comment> GetSortedComments();
    }
}
