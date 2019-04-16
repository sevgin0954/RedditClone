using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.SearchModels.ViewModels
{
    public class SearchResultViewModel
    {
        public string[] KeyWords { get; set; }

        public IEnumerable<SubredditConciseViewModel> Subreddits { get; set; }

        public PostsViewModel Posts { get; set; }
    }
}
