using RedditClone.Common.Enums.SortTypes;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestSubredditService
    {
        Task<SubredditsViewModel> GetOrderedSubredditsFilterByKeyWordsAsync(string[] keyWords, SubredditSortType sortType);
    }
}
