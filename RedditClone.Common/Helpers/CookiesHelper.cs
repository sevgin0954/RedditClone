using Microsoft.AspNetCore.Http;
using RedditClone.Common.Constants;
using System;

namespace RedditClone.Common.Helpers
{
    public static class CookiesHelper
    {
        public static void SetDefaultPostSortTypeCookie(IResponseCookies responseCookies)
        {
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = WebConstants.CookieDefaultValuePostSortType;

            var cookieOptions = GetOptionForDeaultCookies();
            responseCookies.Append(postSortTypeKey, postSortTypeValue, cookieOptions);
        }
        
        public static void SetDefaultPostShowTimeFrameCookie(IResponseCookies responseCookies)
        {
            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = WebConstants.CookieDefaultValuePostShowTimeFrame;

            var cookieOptions = GetOptionForDeaultCookies();
            responseCookies.Append(postTimeFrameKey, postTimeFrameValue, cookieOptions);
        }

        public static void SetDefaultCommentSortTypeCookie(IResponseCookies responseCookies)
        {
            var commentSortTypeKey = WebConstants.CookieKeyCommentSortType;
            var commentSortTypeValue = WebConstants.CookieDefaultValueCommentSortType;

            var cookieOptions = GetOptionForDeaultCookies();
            responseCookies.Append(commentSortTypeKey, commentSortTypeValue, cookieOptions);
        }

        private static CookieOptions GetOptionForDeaultCookies()
        {
            var cookieOptions = new CookieOptions()
            {
                IsEssential = true,
                Expires = DateTimeOffset.MaxValue
            };

            return cookieOptions;
        }
    }
}
