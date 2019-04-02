using Microsoft.AspNetCore.Identity;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Tests.Common;

namespace RedditClone.Tests.UserAccountServiceTests.UserPostServiceTests
{
    public abstract class BaseUserAccountServiceTest : BaseTest
    {
        public IUserAccountService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var mapper = CommonTestMethods.GetMapper();
            var service = new UserAccountService(unitOfWork, userManager, mapper);

            return service;
        }

        public Mock<UserManager<User>> GetMockedUserManager()
        {
            var userStore = new Mock<IUserStore<User>>().Object;
            var mockedUserManager = new Mock<UserManager<User>>(userStore, null, null, null, null, null, null, null, null);

            return mockedUserManager;
        }
    }
}
