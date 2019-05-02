namespace RedditClone.Models
{
    public class VotePost
    {
        public string Id { get; set; }
        
        public int Value { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}