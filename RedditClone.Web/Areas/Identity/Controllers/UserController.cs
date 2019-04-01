using Microsoft.AspNetCore.Mvc;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class UserController : BaseIdentityController
    {
        private readonly IUserAccountService userPostService;

        public UserController(IUserAccountService userPostService)
        {
            this.userPostService = userPostService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await this.userPostService.PrepareIndexModelAsync(this.User);

            return this.View(models);
        }
    }
}