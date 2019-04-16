using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Models.WebModels.SearchModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestSearchService
    {
        Task<SearchResultViewModel> GetSearchResultAsync(
            string[] keyWords,
            SubredditSortType subredditSortType,
            PostSortType postSortType,
            TimeFrameType postTimeFrameType);
    }
}
