using System;
using System.Collections.Generic;

namespace RedditClone.Models
{
    public class Comment
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public DateTime PostDate { get; set; }

        public int VotesCount { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
