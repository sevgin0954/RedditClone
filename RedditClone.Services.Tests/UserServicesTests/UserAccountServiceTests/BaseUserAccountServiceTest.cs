using RedditClone.Data.Interfaces;
using RedditClone.Services.Tests.Common;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.Tests.UserAccountServiceTests.UserPostServiceTests
{
    public abstract class BaseUserAccountServiceTest : BaseTest
    {
        public IUserAccountService GetService(IRedditCloneUnitOfWork unitOfWork)
        {
            var mapper = CommonTestMethods.GetAutoMapper();
            var service = new UserAccountService(unitOfWork, mapper);

            return service;
        }
    }
}
