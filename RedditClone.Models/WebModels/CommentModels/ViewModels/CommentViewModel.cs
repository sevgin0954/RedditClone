using System;
using System.Collections.Generic;

namespace RedditClone.Models.WebModels.CommentModels.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public DateTime PostDate { get; set; }

        public int VotesCount { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
    }
}
