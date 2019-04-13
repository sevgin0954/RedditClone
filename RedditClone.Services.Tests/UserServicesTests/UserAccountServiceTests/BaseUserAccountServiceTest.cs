using RedditClone.Data.Interfaces;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Tests.Common;

namespace RedditClone.Services.Tests.UserAccountServiceTests.UserPostServiceTests
{
    public abstract class BaseUserAccountServiceTest : BaseTest
    {
        public IUserAccountService GetService(IRedditCloneUnitOfWork unitOfWork)
        {
            var mapper = CommonTestMethods.GetMapper();
            var service = new UserAccountService(unitOfWork, mapper);

            return service;
        }
    }
}
