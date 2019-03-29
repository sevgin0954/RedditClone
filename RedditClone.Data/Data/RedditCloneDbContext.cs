using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RedditClone.Data
{
    public class RedditCloneDbContext : IdentityDbContext
    {
        public RedditCloneDbContext(DbContextOptions<RedditCloneDbContext> options)
            : base(options)
        {
        }
    }
}
