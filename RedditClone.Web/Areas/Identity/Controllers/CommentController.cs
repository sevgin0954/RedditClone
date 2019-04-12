using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Models.WebModels.CommentModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class CommentController : BaseIdentityController
    {
        private readonly IUserCommentService userCommentService;

        public CommentController(IUserCommentService userCommentService)
        {
            this.userCommentService = userCommentService;
        }

        [HttpGet]
        public IActionResult AddToPost(string postId)
        {
            var model = new CommentBindingModel()
            {
                SourceId = postId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToPost(CommentBindingModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                this.AddStatusMessage(this.ModelState);
                return this.RedirectToAction("AddToPost", new { postId = model.SourceId });
            }

            var isSubccessfull = await this.userCommentService.AddCommentToPostAsync(this.User, model);
            if (isSubccessfull == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageUnknownError, AlertConstants.AlertTypeDanger);
                return this.RedirectToAction("AddToPost", new { postId = model.SourceId });
            }

            return this.RedirectToAction("Index", "Post", new { postId = model.SourceId });
        }

        [HttpPost]
        public async Task<IActionResult> AddToComment(CommentBindingModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                this.AddStatusMessage(this.ModelState);
                return this.RedirectToAction("Index", "Post", new { postId = model.SourceId });
            }

            var isSubccessfull = await this.userCommentService.AddResponseToCommentAsync(this.User, model);
            if (isSubccessfull == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageUnknownError, AlertConstants.AlertTypeDanger);
                return this.RedirectToAction("AddToPost", new { postId = model.SourceId });
            }

            return this.RedirectToAction("Index", "Post", new { postId = model.SourceId });
        }
    }
}