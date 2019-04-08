using System;

namespace RedditClone.Models.WebModels.PostModels.ViewModels
{
    public class PostConciseViewModel
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }
        public string AuthorUsername { get; set; }

        public string Title { get; set; }

        public string DescriptionConcise { get; set; }

        public DateTime PostDate { get; set; }

        public int VotesCount { get; set; }

        public string SubredditId { get; set; }
        public string SubredditName { get; set; }

        public int CommentsCount { get; set; }
    }
}
