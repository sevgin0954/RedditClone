using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class UserController : BaseIdentityController
    {
        private readonly IUserAccountService userAccountService;

        public UserController(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId)
        {
            var models = await this.userAccountService.PrepareIndexModelAsync(userId);

            if (models == null)
            {
                this.AddStatusMessage(WebConstants.ErrorMessageWrongId, WebConstants.MessageTypeDanger);
                return this.Redirect("/");
            }

            return this.View(models);
        }
    }
}