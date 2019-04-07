using RedditClone.Models.WebModels.UserModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserAccountService
    {
        Task<IEnumerable<UserIndexViewModel>> PrepareIndexModelAsync(string userId);
    }
}
