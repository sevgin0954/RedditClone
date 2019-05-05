using Microsoft.AspNetCore.Http;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserPostService
    {
        Task<PostCreationBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId);

        Task<bool> CreatePostAsync(ClaimsPrincipal user, PostCreationBindingModel model);

        Task<PostsViewModel> GetOrderedPostsAsync(
            ClaimsPrincipal user,
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies);

        Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            ClaimsPrincipal user,
            string postId,
            IRequestCookieCollection requestCookies);
    }
}
