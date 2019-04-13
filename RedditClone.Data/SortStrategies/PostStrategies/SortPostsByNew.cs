﻿using System.Collections.Generic;
using System.Threading.Tasks;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;

namespace RedditClone.Data.SortStrategies.PostStrategies
{
    public class SortPostsByNew : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByNew(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubcribedUserOrderedByNewAsync(userId);
            return dbPosts;
        }

        public async Task<IEnumerable<Post>> GetSortedPostsAsync()
        {
            var dbPosts = await this.unitOfWork.Posts.GetOrderByNewAsync();
            return dbPosts;
        }
    }
}
