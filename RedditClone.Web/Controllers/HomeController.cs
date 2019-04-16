using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Web.Models;

namespace RedditClone.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IQuestPostService questPostService;
        private readonly IUserPostService userPostService;
        private readonly ICookieService cookieService;
        private readonly SignInManager<User> signInManager;

        public HomeController(
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
        public async Task<IActionResult> Index()
        {
            PostsViewModel model = null;

            if (signInManager.IsSignedIn(this.User))
            {
                model = await this.userPostService.GetOrderedPostsAsync(
                    this.User,
                    this.Request.Cookies,
                    this.Response.Cookies);
            }
            else
            {
                model = await this.questPostService.GetOrderedPostsAsync(this.Request.Cookies);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeSortType(string sortType)
        {
            PostSortType postSortType = PostSortType.Best;
            var isParseSuccessfull = Enum.TryParse(sortType, out postSortType);

            if (isParseSuccessfull == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongParameter, AlertConstants.AlertTypeDanger);
            }
            else
            {
                this.cookieService.ChangePostSortTypeCookie(this.Response.Cookies, postSortType);
            }

            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult ChangeTimeFrame(string timeFrame)
        {
            TimeFrameType timeFrameType = TimeFrameType.AllTime;
            var isParseSuccessfull = Enum.TryParse(timeFrame, out timeFrameType);

            if (isParseSuccessfull)
            {
                this.cookieService.ChangePostTimeFrameCookie(this.Response.Cookies, timeFrameType);
            }
            else
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongParameter, AlertConstants.AlertTypeDanger);
            }

            return this.Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
