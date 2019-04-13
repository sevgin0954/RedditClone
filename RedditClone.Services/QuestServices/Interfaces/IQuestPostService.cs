using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestPostService
    {
        Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            string postId, 
            IRequestCookieCollection requestCookies, 
            IResponseCookies responseCookies);

        void ChangeCommentSortType(IResponseCookies responseCookies, SortType sortType);
    }
}
