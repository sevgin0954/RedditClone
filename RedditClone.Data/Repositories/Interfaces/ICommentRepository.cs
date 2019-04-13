using RedditClone.Data.Repositories.Generic.Interfaces;
using RedditClone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostOrderedByNewAsync(string postId);
        Task<IEnumerable<Comment>> GetByPostOrderedByTopAsync(string postId);
        Task<IEnumerable<Comment>> GetByPostOrderedByControversialAsync(string postId);
        Task<IEnumerable<Comment>> GetByPostOrderedByBestAsync(string postId);
    }
}
