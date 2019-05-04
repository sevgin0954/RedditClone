using RedditClone.Data.Interfaces;
using RedditClone.Services.QuestServices;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.Tests.Common;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestSubredditServiceTests
{
    public abstract class BaseQuestSubredditServiceTest : BaseTest
    {
        public IQuestSubredditService GetService(IRedditCloneUnitOfWork unitOfWork)
        {
            var autoMapper = CommonTestMethods.GetAutoMapper();
            var service = new QuestSubredditService(unitOfWork, autoMapper);

            return service;
        }
    }
}
