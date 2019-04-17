﻿using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using System;
using System.Linq;

namespace RedditClone.Data.SortStrategies.PostOrders
{
    public class SortPostsByBest : ISortPostsStrategy
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;

        public SortPostsByBest(IRedditCloneUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Post> GetSortedPosts()
        {
            var postsQueryable = this.unitOfWork.Posts
                .GetAllAsQueryable()
                .OrderByDescending(p => p.UpVotesCount - p.DownVotesCount)
                .ThenByDescending(p => p.PostDate);

            return postsQueryable;
        }
    }
}
