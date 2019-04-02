using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
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
                this.AddStatusMessage(ModelState);
                return this.Redirect("/");
            }

            var result = await this.userSubredditService.CreateSubredditAsync(model, this.User);

            if (result == false)
            {
                this.AddStatusMessage(WebConstants.ErrorMessageSubredditNameTaken, WebConstants.MessageTypeDanger);
                return this.RedirectToAction("Create");
            }

            this.AddStatusMessage(WebConstants.MessageSubredditCreated, WebConstants.MessageTypeSuccess);
            return this.Redirect("/");
        }
    }
}