using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.PostModels.ViewModels
{
    public class PostsViewModel
    {
        public TimeFrameType? PostTimeFrameType { get; set; }

        public PostSortType PostSortType { get; set; }

        public IEnumerable<PostConciseViewModel> Posts { get; set; } = new List<PostConciseViewModel>();
    }
}
