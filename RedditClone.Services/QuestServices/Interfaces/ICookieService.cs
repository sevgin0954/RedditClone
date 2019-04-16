using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface ICookieService
    {
        PostSortType GetPostSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies);
        CommentSortType GetCommentSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies);
        TimeFrameType GetPostTimeFrameTypeFromCookieOrDefault(IRequestCookieCollection requestCookies);
        void ChangeCommentSortTypeCookie(IResponseCookies responseCookies, CommentSortType commentSortType);
        void ChangePostSortTypeCookie(IResponseCookies responseCookies, PostSortType postSortType);
        void ChangePostTimeFrameCookie(IResponseCookies responseCookies, TimeFrameType timeFrameType);
    }
}
