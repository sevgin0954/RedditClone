using Microsoft.AspNetCore.Mvc;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class SubredditController : BaseIdentityController
    {
        private readonly IUserSubredditService userSubredditService;

        public SubredditController(IUserSubredditService userSubredditService)
        {
            this.userSubredditService = userSubredditService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = this.userSubredditService.PrepareModelForCreating(this.User);

            return View(model);
        }
    }
}