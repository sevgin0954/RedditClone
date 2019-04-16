using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Services.QuestServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Controllers
{
    public class SubredditController : BaseController
    {
        private readonly IQuestPostService questPostService;

        public SubredditController(IQuestPostService questPostService)
        {
            this.questPostService = questPostService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string subredditId)
        {
            var model = await this.questPostService.GetOrderedPostsBySubredditAsync(
                subredditId,
                this.Request.Cookies);

            if (model == null)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongId, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            return this.View(model);
        }
    }
}