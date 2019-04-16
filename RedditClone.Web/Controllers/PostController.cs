using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Services.QuestServices.Interfaces;
using System;
using System.Threading.Tasks;

namespace RedditClone.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IQuestPostService questPostService;
        private readonly ICookieService cookieService;

        public PostController(IQuestPostService questPostService, ICookieService cookieService)
        {
            this.questPostService = questPostService;
            this.cookieService = cookieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string postId)
        {
            if (postId == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            var model = await this.questPostService.GetPostWithOrderedCommentsAsync(postId, this.Request.Cookies);
            if (model == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeCommentSortType(string sortType, string postId)
        {
            CommentSortType commentSortType = CommentSortType.Best;
            var isParseSuccessfull = Enum.TryParse(sortType, out commentSortType);

            if (isParseSuccessfull == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongParameter, AlertConstants.AlertTypeDanger);
            }
            else
            {
                this.cookieService.ChangeCommentSortTypeCookie(this.Response.Cookies, commentSortType);
            }

            return this.RedirectToAction("Index", new { postId });
        }
    }
}