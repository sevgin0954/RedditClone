﻿using System;
using System.Collections.Generic;

namespace RedditClone.Models
{
    public class Subreddit
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<UserSubreddit> SubscribedUsers { get; set; } = new List<UserSubreddit>();
    }
}