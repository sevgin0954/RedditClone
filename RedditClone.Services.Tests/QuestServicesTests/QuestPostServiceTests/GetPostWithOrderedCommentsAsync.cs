﻿using Microsoft.AspNetCore.Http;
using Moq;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetPostWithOrderedCommentsAsync : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithoutIncorrectPostId_ShouldReturnNull()
        {
            var incorrectPostId = Guid.NewGuid().ToString();

            var model = await this.CallGetPostWithOrderedCommentsAsync(incorrectPostId);

            Assert.Null(model);
        }

        [Fact]
        public async Task WithoutComments_ShouldReturnModelWithEmptyCommentsCollection()
        {
            var dbPost = new Post();

            var model = await this.CallGetPostWithOrderedCommentsAsyncWithPost(dbPost);

            Assert.Empty(model.Comments);
        }

        [Fact]
        public async Task WithCommentWithReply_ShouldReturnModelWithCommentAndReply()
        {
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbCommentReplie = new Comment();
            dbComment.Replies.Add(dbCommentReplie);
            dbPost.Comments.Add(dbComment);

            var model = await this.CallGetPostWithOrderedCommentsAsyncWithPost(dbPost);
            var modelFirstComment = model.Comments.First();
            var firstCommentReply = modelFirstComment.Replies.First();

            Assert.Equal(dbComment.Id, modelFirstComment.Id);
            Assert.Equal(dbCommentReplie.Id, firstCommentReply.Id);
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsync(string postId)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var service = this.GetService(unitOfWork);

            var requestCookies = new Mock<IRequestCookieCollection>().Object;
            var model = await service.GetPostWithOrderedCommentsAsync(postId, requestCookies);

            return model;
        }

        private async Task<PostViewModel> CallGetPostWithOrderedCommentsAsyncWithPost(Post post)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);

            var requestCookies = new Mock<IRequestCookieCollection>().Object;
            var model = await service.GetPostWithOrderedCommentsAsync(post.Id, requestCookies);

            return model;
        }
    }
}
