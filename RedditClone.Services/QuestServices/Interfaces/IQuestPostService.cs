using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface IQuestPostService
    {
        Task<PostsViewModel> GetOrderedPostsAsync(
            IRequestCookieCollection requestCookies);

        Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            string postId, 
            IRequestCookieCollection requestCookies);

        Task<PostsViewModel> GetOrderedPostsBySubredditAsync(
            string subredditId,
            IRequestCookieCollection requestCookies);

        Task<PostsViewModel> GetOrderedPostsByKeyWordsAsync(
            string[] keyWords,
            PostSortType sortType,
            TimeFrameType timeFrameType);
    }
}
