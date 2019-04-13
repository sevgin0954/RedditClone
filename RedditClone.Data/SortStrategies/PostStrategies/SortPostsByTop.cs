﻿using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostOrders;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Data.SortStrategies.PostStrategies
{
    public class SortPostsByTop : BaseTimeDependentPostSortingStrategy, ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByTop(IRedditCloneUnitOfWork unitOfWork, TimeSpan timeFrame)
            : base(timeFrame)
        {
            this.unitOfWork = unitOfWork;
        }

        public override async Task<IEnumerable<Post>> GetSortedPostsByUserAsync(string userId)
        {
            var dbPosts = await this.unitOfWork.Posts.GetBySubcribedUserOrderedByTopAsync(userId, this.TimeFrame);
            return dbPosts;
        }

        public override async Task<IEnumerable<Post>> GetSortedPostsAsync()
        {
            var dbPosts = await this.unitOfWork.Posts.GetOrderedByTopAsync(this.TimeFrame);
            return dbPosts;
        }
    }
}
