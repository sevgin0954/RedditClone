using RedditClone.Common.Enums.SortTypes;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.SubredditModels.ViewModels
{
    public class SubredditsViewModel
    {
        public SubredditSortType SubrreditSortType { get; set; }

        public IEnumerable<SubredditConciseViewModel> Subreddits { get; set; }
    }
}
