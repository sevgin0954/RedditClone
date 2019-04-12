using RedditClone.Models.WebModels.CommentModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserCommentService
    {
        Task<bool> AddCommentToPostAsync(ClaimsPrincipal user, CommentBindingModel model);
        Task<bool> AddResponseToCommentAsync(ClaimsPrincipal user, CommentBindingModel model);
    }
}
