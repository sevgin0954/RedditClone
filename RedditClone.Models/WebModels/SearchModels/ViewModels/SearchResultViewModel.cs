using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;

namespace RedditClone.Models.WebModels.SearchModels.ViewModels
{
    public class SearchResultViewModel
    {
        public string KeyWords { get; set; }

        public SubredditsViewModel Subreddits { get; set; }

        public PostsViewModel Posts { get; set; }
    }
}
