namespace RedditClone.Models
{
    public class UserSubreddit
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string SubredditId { get; set; }
        public Subreddit Subreddit { get; set; }
    }
}
