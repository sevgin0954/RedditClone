using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Services.QuestServices.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IQuestSearchService questSearchService;

        public SearchController(IQuestSearchService questSearchService)
        {
            this.questSearchService = questSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string keyWords, 
            string subredditSortName,
            string postSortName,
            string postTimeFrameName)
        {
            var keyWordsSplited = keyWords
                ?.Split(" ")
                .Where(w => w.Length > 2)
                .ToArray();

            if (keyWordsSplited == null || keyWordsSplited.Length == 0)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageWrongParameter, AlertConstants.AlertTypeDanger);
                return this.Redirect("/");
            }

            var subredditSortType = SubredditSortType.Top;
            if (Enum.TryParse(subredditSortName, out subredditSortType) == false)
            {
                subredditSortType = SubredditSortType.Top;
            }

            var postSortType = PostSortType.Best;
            if (Enum.TryParse(postSortName, out postSortType) == false)
            {
                postSortType = PostSortType.Best;
            }

            var postTimeFrameType = TimeFrameType.AllTime;
            if (Enum.TryParse(postTimeFrameName, out postTimeFrameType) == false)
            {
                postTimeFrameType = TimeFrameType.AllTime;
            }

            var model = await this.questSearchService
                .GetSearchResultAsync(keyWordsSplited, subredditSortType, postSortType, postTimeFrameType);

            return View(model);
        }
    }
}