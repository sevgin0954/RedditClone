namespace RedditClone.Common.Constants
{
    public static class ModelsConstants
    {
        public const int DescriptionPostMinLength = 0;
        public const int DescriptionPostMaxLength = 9000;
        public const int DescriptionAccountPreviewLength = 100;
        public const int DescriptionIndexPreviewLength = 500;
        public const int DescriptionSubredditMinLength = 20;
        public const int DescriptionSubredditMaxLength = 300;
        public const int DescriptionCommentMinLength = 10;
        public const int DescriptionCommentMaxLength = 9000;

        public const int TitlePostPreviewLength = 20;
        public const int TitlePostMinLength = 7;
        public const int TitlePostMaxLength = 150;
        public const int TitleSubredditMinLength = 3;
        public const int TitleSubredditMaxLength = 100;

        public const string ActionNameComment = "commented on";
        public const string ActionNamePost = "posted";

        public const string SelectListGroupNameCreatedSubreddits = "Created";
        public const string SelectListGroupNameSubscribedSubreddits = "Subscribtions";
        public const string SelectListItemNameEmpty = "None";
    }
}
