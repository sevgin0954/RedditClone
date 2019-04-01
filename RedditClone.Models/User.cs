using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RedditClone.Models
{
    public class User : IdentityUser
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Subreddit> CreatedSubreddits { get; set; } = new List<Subreddit>();

        public ICollection<UserSubreddit> SubscribedSubreddits { get; set; } = new List<UserSubreddit>();
    }
}
