using Microsoft.AspNetCore.Identity;
using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Tests.UserServicesTests.UserPostServiceTests
{
    public abstract class BaseUserPostServiceTest : BaseTest
    {
        public IUserPostService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var service = new UserPostService(unitOfWork, userManager);

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
