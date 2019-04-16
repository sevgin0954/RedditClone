using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestSubredditService
    {
        Task<IEnumerable<SubredditConciseViewModel>> GetOrderedSubredditsByKeyWords(
            string[] keyWords,
            SubredditSortType sortType);
    }
}
