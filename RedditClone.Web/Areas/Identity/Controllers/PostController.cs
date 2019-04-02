using Microsoft.AspNetCore.Mvc;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class PostController : BaseIdentityController
    {
        private readonly IUserPostService userPostService;

        public PostController(IUserPostService userPostService)
        {
            this.userPostService = userPostService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string subredditId)
        {
            var model = await this.userPostService.PrepareModelForCreatingAsync(this.User, subredditId);

            return View(model);
        }
    }
}