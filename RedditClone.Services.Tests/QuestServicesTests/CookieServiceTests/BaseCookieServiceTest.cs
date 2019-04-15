using RedditClone.Services.QuestServices;
using RedditClone.Services.QuestServices.Interfaces;

namespace RedditClone.Services.Tests.QuestServicesTests.CookieServiceTests
{
    public class BaseCookieServiceTest : BaseTest
    {
        public ICookieService GetService()
        {
            return new CookieService();
        }
    }
}
