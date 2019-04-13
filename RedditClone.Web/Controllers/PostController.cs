using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Services.QuestServices.Interfaces;
using System;
using System.Threading.Tasks;

namespace RedditClone.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IQuestPostService questPostService;

        public PostController(IQuestPostService questPostService)
        {
            this.questPostService = questPostService;
        }

        public async Task<IActionResult> Index(string postId)
        {
            if (postId == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            var model = await this.questPostService.GetPostWithOrderedCommentsAsync(postId, this.Request.Cookies, this.Response.Cookies);
            if (model == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeSortType(string sortType, string postId)
        {
            SortType postSortType = SortType.Best;
            var isParseSuccessfull = Enum.TryParse(sortType, out postSortType);

            if (isParseSuccessfull == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongParameter, AlertConstants.AlertTypeDanger);
            }
            else
            {
                this.questPostService.ChangeCommentSortType(this.Response.Cookies, postSortType);
            }

            return this.RedirectToAction("Index", new { postId });
        }
    }
}