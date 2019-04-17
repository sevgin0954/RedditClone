using Moq;
using RedditClone.Services.QuestServices;
using RedditClone.Services.QuestServices.Interfaces;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestSearchServiceTests
{
    public class BaseQuestSearchServiceTest : BaseTest
    {
        public IQuestSearchService GetService()
        {
            var questPostService = new Mock<IQuestPostService>().Object;
            var questSubredditService = new Mock<IQuestSubredditService>().Object;

            var service = new QuestSearchService(questPostService, questSubredditService);
            return service;
        }
    }
}
