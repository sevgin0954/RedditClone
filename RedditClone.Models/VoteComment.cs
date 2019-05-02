namespace RedditClone.Models
{
    public class VoteComment
    {
        public string Id { get; set; }

        public int Value { get; set; }

        public string CommentId { get; set; }
        public Comment Comment { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
