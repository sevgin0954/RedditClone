using RedditClone.Common.Enums;

namespace RedditClone.Common.Constants
{
    public abstract class WebConstants
    {
        public const string StatusMessagePrefix = "___customStatusMessage";
        public const string StatusMessageTypeKey = "___type_customStatusMessage";

        public const string MessageTypeSuccess = "success";
        public const string MessageTypeDanger = "danger";

        public const string ErrorMessageSubredditNameTaken = "Subreddit with that name already exist";
        public const string ErrorMessageUnknownError = "Unknown error has occurred";
        public const string ErrorMessageWrongId = "Incorrect Id";
        public const string ErrorMessageWrongParameter = "Incorrect parameter";

        public const string MessageSubredditCreated = "Subreddit created successfully";
        public const string MessagePostCreated = "Post created successfully";

        public const string IdentityAreaName = "Identity";

        public const string CookieKeyPostSortType = "PostSortType";
        public const string CookieDefaultValuePostSortType = "Best";

        public const string CookieKeyPostShowTimeFrame = "PostShowTimeFrame";
        public const string CookieDefaultValuePostShowTimeFrame = "PastDay";
    }
}
