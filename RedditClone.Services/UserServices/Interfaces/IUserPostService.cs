using RedditClone.Models.WebModels.PostModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserPostService
    {
        Task<CreationPostBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId);
    }
}
