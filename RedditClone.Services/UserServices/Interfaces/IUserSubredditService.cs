using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserSubredditService
    {
        SubredditCreationBindingModel PrepareModelForCreating();

        Task<bool> CreateSubredditAsync(SubredditCreationBindingModel model, ClaimsPrincipal user);
    }
}
