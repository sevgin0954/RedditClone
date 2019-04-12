using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Services.QuestServices.Interfaces;
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

        public async Task<IActionResult> Index(string postId, SortType sortType = SortType.Best)
        {
            if (postId == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            var model = await this.questPostService.GetPostModelAsync(postId, sortType);
            if (model == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            return View(model);
        }
    }
}