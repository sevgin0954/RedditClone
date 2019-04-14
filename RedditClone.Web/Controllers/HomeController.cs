using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Models;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
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
            IndexViewModel model = null;

            if (signInManager.IsSignedIn(this.User))
            {
                model = await this.userPostService.GetOrderedPostsAsync(
                    this.User,
                    this.Request.Cookies,
                    this.Response.Cookies);
            }
            else
            {
                model = await this.questPostService.GetOrderedPostsAsync(this.Request.Cookies, this.Response.Cookies);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeSortType(string sortType)
        {
            SortType postSortType = SortType.Best;
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
            PostShowTimeFrame postTimeFrame = PostShowTimeFrame.AllTime;
            var isParseSuccessfull = Enum.TryParse(timeFrame, out postTimeFrame);

            if (isParseSuccessfull)
            {
                this.cookieService.ChangePostTimeFrameCookie(this.Response.Cookies, postTimeFrame);
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
