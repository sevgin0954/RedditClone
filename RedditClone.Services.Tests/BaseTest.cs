using Microsoft.EntityFrameworkCore;
using RedditClone.Data;
using RedditClone.Data.Interfaces;
using System;

namespace RedditClone.Services.Tests
{
    public abstract class BaseTest
    {
        protected IRedditCloneUnitOfWork GetRedditCloneUnitOfWork()
        {
            var dbContext = this.GetDbContext();
            var redditCloneUOW = new RedditCloneUnitOfWork(dbContext);

            return redditCloneUOW;
        }

        private RedditCloneDbContext GetDbContext()
        {
            var options = this.GetDbContextOptions();
            var dbContext = new RedditCloneDbContext(options);

            return dbContext;
        }

        private DbContextOptions<RedditCloneDbContext> GetDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<RedditCloneDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }
    }
}
