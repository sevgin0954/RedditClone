using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Models.WebModels.PostModels.BindingModels;
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

        [HttpPost]
        public async Task<IActionResult> Create(PostCreationBindingModel model)
        {
            if (ModelState.IsValid == false)
            {
                this.AddStatusMessage(ModelState);
                return Redirect("/");
            }

            var result = await this.userPostService.CreatePostAsync(this.User, model);

            if (result == false)
            {
                this.AddStatusMessage(WebConstants.ErrorMessageUnknownError, WebConstants.MessageTypeDanger);
                return Redirect("/");
            }

            this.AddStatusMessage(WebConstants.MessagePostCreated, WebConstants.MessageTypeSuccess);
            return Redirect("/");
        }
    }
}