using Moq;
using RedditClone.CustomMapper;
using RedditClone.Data.Interfaces;
using RedditClone.Services.QuestServices;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.Tests.Common;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class BaseQuestPostServiceTest : BaseTest
    {
        public IQuestPostService GetService(IRedditCloneUnitOfWork unitOfWork)
        {
            var cookieSerive = new Mock<ICookieService>().Object;
            var autoMapper = CommonTestMethods.GetAutoMapper();
            var postMapper = new PostMapper(autoMapper);
            var service = new QuestPostService(unitOfWork, cookieSerive, postMapper);

            return service;
        }
    }
}
