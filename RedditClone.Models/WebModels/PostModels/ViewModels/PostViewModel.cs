using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using System;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.PostModels.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PostDate { get; set; }

        public int VotesCount { get; set; }

        public int CommentsCount { get; set; }

        public string SubredditId { get; set; }
        public Subreddit Subreddit { get; set; }

        public SortType SelectedSortType { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
