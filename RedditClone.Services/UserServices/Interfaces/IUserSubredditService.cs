using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using System.Security.Claims;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserSubredditService
    {
        SubredditCreationBindingModel PrepareModelForCreating(ClaimsPrincipal user);
    }
}
