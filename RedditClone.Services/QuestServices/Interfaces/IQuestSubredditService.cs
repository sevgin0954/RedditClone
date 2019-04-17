using RedditClone.Common.Enums.SortTypes;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestSubredditService
    {
        Task<SubredditsViewModel> GetOrderedSubredditsByKeyWords(string[] keyWords, SubredditSortType sortType);
    }
}
