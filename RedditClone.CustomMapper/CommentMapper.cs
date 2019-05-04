using RedditClone.CustomMapper.Interfaces;
using RedditClone.Models;
using System;

namespace RedditClone.CustomMapper
{
    public class CommentMapper : ICommentMapper
    {
        public Comment MapComment(string postId, string description, string dbUserId)
        {
            var comment = new Comment()
            {
                PostId = postId,
                AuthorId = dbUserId,
                Description = description,
                PostDate = DateTime.UtcNow
            };

            return comment;
        }
    }
}
