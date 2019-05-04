using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestSearchServiceTests
{
    public class GetSearchResultAsyncTests : BaseQuestSearchServiceTest
    {
        [Fact]
        public async Task WithKeyWords_ShouldReturnModelWithCombinedInOneStringKeywords()
        {
            var service = this.GetService();

            var keyWords = new string[] { "keyWord1", "keyWord2" };
            var model = await service.GetSearchResultAsync(
                keyWords, 
                SubredditSortType.New, 
                PostSortType.New, 
                TimeFrameType.AllTime);

            var expectedModelKeyWords = string.Join(" ", keyWords);

            Assert.Equal(model.KeyWords, expectedModelKeyWords);
        }
    }
}
