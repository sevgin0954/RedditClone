using RedditClone.Models;
using System.Collections.Generic;
using System.Linq;

namespace RedditClone.Data.Helpers
{
    public static class CountComments
    {
        public static int Count(IEnumerable<Comment> comments)
        {
            if (comments.Count() == 0)
            {
                return 0;
            }

            int totalCommentsCount = comments.Count();

            foreach (var comment in comments)
            {
                int RepliesCommentsCount = Count(comment.Replies);
                totalCommentsCount += RepliesCommentsCount;
            }

            return totalCommentsCount;
        }
    }
}
