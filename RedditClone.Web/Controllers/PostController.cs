using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.UserServices.Interfaces;
using System;
using System.Threading.Tasks;

namespace RedditClone.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IQuestPostService questPostService;
        private readonly IUserPostService userPostService;
        private readonly ICookieService cookieService;
        private readonly SignInManager<User> signInManager;

        public PostController(
            IQuestPostService questPostService, 
            IUserPostService userPostService,
            ICookieService cookieService, 
            SignInManager<User> signInManager)
        {
            this.questPostService = questPostService;
            this.userPostService = userPostService;
            this.cookieService = cookieService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string postId)
        {
            if (postId == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            var model = new PostViewModel();
            if (this.signInManager.IsSignedIn(this.User))
            {
                model = await this.userPostService.GetPostWithOrderedCommentsAsync(this.User, postId, this.Request.Cookies);
            }
            else
            {
                model = await this.questPostService.GetPostWithOrderedCommentsAsync(postId, this.Request.Cookies);
            }
            
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