using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestPostService
    {
        Task<PostViewModel> GetPostModelAsync(string postId, SortType sortType);
    }
}
