using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums;

namespace RedditClone.Services.QuestServices.Interfaces
{
    public interface ICookieService
    {
        SortType GetPostSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies);
        SortType GetCommentSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies);
        PostShowTimeFrame GetPostShowTimeFrameFromCookieOrDefault(IRequestCookieCollection requestCookies);
        void ChangeCommentSortTypeCookie(IResponseCookies responseCookies, SortType sortType);
        void ChangePostSortTypeCookie(IResponseCookies responseCookies, SortType postSortType);
        void ChangePostTimeFrameCookie(IResponseCookies responseCookies, PostShowTimeFrame postShowTimeFrame);
    }
}
