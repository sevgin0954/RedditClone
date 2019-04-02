using Microsoft.AspNetCore.Mvc;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

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
            var model = this.userSubredditService.PrepareModelForCreating();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubredditCreationBindingModel model)
        {
            if (ModelState.IsValid == false)
            {

            }

            var result = await this.userSubredditService.CreateSubredditAsync(model, this.User);

            if (result == false)
            {

            }

            return this.Redirect("/");
        }
    }
}