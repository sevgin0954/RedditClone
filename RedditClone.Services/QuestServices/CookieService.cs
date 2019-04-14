using Microsoft.AspNetCore.Http;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Services.QuestServices.Interfaces;
using System;

namespace RedditClone.Services.QuestServices
{
    public class CookieService : ICookieService
    {
        public SortType GetPostSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = requestCookies[postSortTypeKey];

            var postSortType = SortType.Best;
            if (Enum.TryParse(postSortTypeValue, out postSortType) == false)
            {
                postSortType = Enum.Parse<SortType>(WebConstants.CookieDefaultValuePostSortType);
            }

            return postSortType;
        }

        public SortType GetCommentSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var commentSortTypeKey = WebConstants.CookieKeyCommentSortType;
            var commentSortTypeValue = requestCookies[commentSortTypeKey];

            var commentSortType = SortType.Best;
            if (Enum.TryParse(commentSortTypeValue, out commentSortType) == false)
            {
                commentSortType = Enum.Parse<SortType>(WebConstants.CookieDefaultValueCommentSortType);
            }

            return commentSortType;
        }

        public PostShowTimeFrame GetPostShowTimeFrameFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postTimeFrameValue = requestCookies[postTimeFrameKey];

            var postShowTimeFrame = PostShowTimeFrame.PastDay;
            if (Enum.TryParse(postTimeFrameValue, out postShowTimeFrame) == false)
            {
                postShowTimeFrame = Enum.Parse<PostShowTimeFrame>(WebConstants.CookieDefaultValuePostShowTimeFrame);
            }

            return postShowTimeFrame;
        }

        public void ChangeCommentSortTypeCookie(IResponseCookies responseCookies, SortType sortType)
        {
            var sortTypeKey = WebConstants.CookieKeyCommentSortType;
            var sortTypeValue = sortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        public void ChangePostSortTypeCookie(IResponseCookies responseCookies, SortType postSortType)
        {
            var sortTypeKey = WebConstants.CookieKeyPostSortType;
            var sortTypeValue = postSortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        public void ChangePostTimeFrameCookie(IResponseCookies responseCookies, PostShowTimeFrame postShowTimeFrame)
        {
            var timeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var timeFrameValue = postShowTimeFrame.ToString();

            responseCookies.Append(timeFrameKey, timeFrameValue);
        }
    }
}
