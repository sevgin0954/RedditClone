namespace RedditClone.Common.Constants
{
    public abstract class ModelsConstants
    {
        public const int DescriptionPostMinLength = 0;
        public const int DescriptionPostMaxLength = 9000;
        public const int DescriptionPreviewLength = 100;

        public const int TitlePostPreviewLength = 50;
        public const int TitlePostMinLength = 7;
        public const int TitlePostMaxLength = 150;

        public const string ActionNameComment = "commented on";
        public const string ActionNamePost = "posted";

        public const string SelectListGroupNameCreatedSubreddits = "Created";
        public const string SelectListGroupNameSubscribedSubreddits = "Subscribtions";

        public const string SelectListItemNameEmpty = "None";
    }
}
