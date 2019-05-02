using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserVotePostService
    {
        Task<bool> AddDownVoteToPostAsync(string postId, ClaimsPrincipal user);
        Task<bool> AddUpVoteToPostAsync(string postId, ClaimsPrincipal user);
        Task<bool> RemoveDownVoteToPostAsync(string postId, ClaimsPrincipal user);
        Task<bool> RemoveUpVoteToPostAsync(string postId, ClaimsPrincipal user);
    }
}
