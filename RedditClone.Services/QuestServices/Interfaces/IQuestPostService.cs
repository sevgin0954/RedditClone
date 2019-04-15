using Microsoft.AspNetCore.Http;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestPostService
    {
        Task<IndexViewModel> GetOrderedPostsAsync(
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies);

        Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            string postId, 
            IRequestCookieCollection requestCookies, 
            IResponseCookies responseCookies);

        Task<IndexViewModel> GetOrderedPostsBySubredditAsync(
            string subredditId,
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies);
    }
}
