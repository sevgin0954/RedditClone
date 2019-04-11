using RedditClone.Common.Enums;
using RedditClone.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetPostModelAsyncTests : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithoutIncorrectPostId_ShouldReturnNull()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var service = this.GetService(unitOfWork);

            var randomPostId = Guid.NewGuid().ToString();
            var result = await service.GetPostModelAsync(randomPostId, SortType.Best);

            Assert.Null(result);
        }

        [Fact]
        public async Task WithoutComments_ShouldReturnModelWithZeroCommentsCount()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost = new Post();
            unitOfWork.Posts.Add(dbPost);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);

            var model = await service.GetPostModelAsync(dbPost.Id, SortType.Best);

            Assert.Equal(0, model.CommentsCount);
        }

        [Fact]
        public async Task WithCommentsWithReplies_ShouldReturnModelWithCorrectCommentsCount()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbCommentReplie = new Comment();
            unitOfWork.Posts.Add(dbPost);
            dbPost.Comments.Add(dbComment);
            dbComment.Replies.Add(dbCommentReplie);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork);

            var model = await service.GetPostModelAsync(dbPost.Id, SortType.Best);

            Assert.Equal(2, model.CommentsCount);
        }
    }
}
