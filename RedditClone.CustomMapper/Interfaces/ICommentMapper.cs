using RedditClone.Models;

namespace RedditClone.CustomMapper.Interfaces
{
    public interface ICommentMapper
    {
        Comment MapComment(string postId, string description, string dbUserId);
    }
}
