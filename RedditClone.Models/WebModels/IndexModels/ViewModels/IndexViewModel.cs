using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.IndexModels.ViewModels
{
    public class IndexViewModel
    {
        public PostShowTimeFrame? PostShowTimeFrame { get; set; }

        public SortType PostSortType { get; set; }

        public IEnumerable<PostConciseViewModel> Posts { get; set; } = new List<PostConciseViewModel>();
    }
}
