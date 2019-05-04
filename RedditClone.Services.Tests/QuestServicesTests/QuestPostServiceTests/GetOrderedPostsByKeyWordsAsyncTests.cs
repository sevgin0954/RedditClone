using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class GetOrderedPostsByKeyWordsAsyncTests : BaseQuestPostServiceTest
    {
        [Fact]
        public async Task WithKeyWord_ShouldReturnModelWithFilteredPosts()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbPost1 = this.CreatePost("Title");
            var dbPost2 = this.CreatePost("New GTA 5 Game");
            this.AddPostsToUnitOfWork(unitOfWork, dbPost1, dbPost2);

            var service = this.GetService(unitOfWork);

            var keyWords = new string[] { "game" };
            var model = await service.GetOrderedPostsByKeyWordsAsync(keyWords, PostSortType.Best, TimeFrameType.AllTime);
            var modelPosts = model.Posts;

            Assert.Single(modelPosts);
            Assert.Equal(dbPost2.Id, modelPosts.First().Id);
        }

        private Post CreatePost(string title)
        {
            var post = new Post()
            {
                Title = title
            };

            return post;
        }

        private void AddPostsToUnitOfWork(IRedditCloneUnitOfWork unitOfWork, params Post[] posts)
        {
            unitOfWork.Posts.AddRange(posts);
            unitOfWork.Complete();
        }
    }
}
