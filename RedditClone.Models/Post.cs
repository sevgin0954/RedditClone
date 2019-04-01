using System;
using System.Collections.Generic;

namespace RedditClone.Models
{
    public class Post
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PostDate { get; set; }

        public int VotesCount { get; set; }

        public string SubredditId { get; set; }
        public Subreddit Subreddit { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
