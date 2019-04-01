using AutoMapper;
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
        private readonly IMapper mapper;

        public BaseUserAccountServiceTest()
        {
            this.mapper = CommonTestMethods.GetMapper();
        }

        public IUserAccountService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var service = new UserAccountService(unitOfWork, userManager, this.mapper);

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
