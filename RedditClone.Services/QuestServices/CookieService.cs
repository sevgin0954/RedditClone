using Microsoft.AspNetCore.Http;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Services.QuestServices.Interfaces;
using System;

namespace RedditClone.Services.QuestServices
{
    public class CookieService : ICookieService
    {
        public PostSortType GetPostSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postSortTypeValue = requestCookies[postSortTypeKey];

            var postSortType = PostSortType.Best;
            if (Enum.TryParse(postSortTypeValue, out postSortType) == false)
            {
                postSortType = Enum.Parse<PostSortType>(WebConstants.CookieDefaultValuePostSortType);
            }

            return postSortType;
        }

        public CommentSortType GetCommentSortTypeFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var commentSortTypeKey = WebConstants.CookieKeyCommentSortType;
            var commentSortTypeValue = requestCookies[commentSortTypeKey];

            var commentSortType = CommentSortType.Best;
            if (Enum.TryParse(commentSortTypeValue, out commentSortType) == false)
            {
                commentSortType = Enum.Parse<CommentSortType>(WebConstants.CookieDefaultValueCommentSortType);
            }

            return commentSortType;
        }

        public TimeFrameType GetPostTimeFrameTypeFromCookieOrDefault(IRequestCookieCollection requestCookies)
        {
            var postTimeFrameKey = WebConstants.CookieKeyPostTimeFrameType;
            var postTimeFrameValue = requestCookies[postTimeFrameKey];

            var postTimeFrameType = TimeFrameType.PastDay;
            if (Enum.TryParse(postTimeFrameValue, out postTimeFrameType) == false)
            {
                postTimeFrameType = Enum.Parse<TimeFrameType>(WebConstants.CookieDefaultValuePostTimeFrameType);
            }

            return postTimeFrameType;
        }

        public void ChangeCommentSortTypeCookie(IResponseCookies responseCookies, CommentSortType commentSortType)
        {
            var sortTypeKey = WebConstants.CookieKeyCommentSortType;
            var sortTypeValue = commentSortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        public void ChangePostSortTypeCookie(IResponseCookies responseCookies, PostSortType postSortType)
        {
            var sortTypeKey = WebConstants.CookieKeyPostSortType;
            var sortTypeValue = postSortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        public void ChangePostTimeFrameCookie(IResponseCookies responseCookies, TimeFrameType timeFrameType)
        {
            var timeFrameKey = WebConstants.CookieKeyPostTimeFrameType;
            var timeFrameValue = timeFrameType.ToString();

            responseCookies.Append(timeFrameKey, timeFrameValue);
        }
    }
}
