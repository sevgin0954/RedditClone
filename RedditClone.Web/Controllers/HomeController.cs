using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Models;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Web.Models;

namespace RedditClone.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserPostService userPostService;
        private readonly SignInManager<User> signInManager;

        public HomeController(IUserPostService userPostService, SignInManager<User> signInManager)
        {
            this.userPostService = userPostService;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel model = await this.userPostService.GetOrderedPostsAsync(
                    this.User,
                    this.Request.Cookies,
                    this.Response.Cookies);

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeSortType(string sortType)
        {
            PostSortType postSortType = PostSortType.Best;
            var result = Enum.TryParse<PostSortType>(sortType, out postSortType);

            if (result == false)
            {
                this.AddStatusMessage(WebConstants.ErrorMessageWrongParameter, WebConstants.MessageTypeDanger);
            }
            else
            {
                this.userPostService.ChangePostSortType(this.Response.Cookies, postSortType);
            }

            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult ChangeTimeFrame(string timeFrame)
        {
            PostShowTimeFrame postTimeFrame = PostShowTimeFrame.AllTime;
            var isParseSuccessfull = Enum.TryParse<PostShowTimeFrame>(timeFrame, out postTimeFrame);

            if (isParseSuccessfull)
            {
                this.userPostService.ChangePostTimeFrame(this.Response.Cookies, postTimeFrame);
            }
            else
            {
                this.AddStatusMessage(WebConstants.ErrorMessageWrongParameter, WebConstants.MessageTypeDanger);
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
