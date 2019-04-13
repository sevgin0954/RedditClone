using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Tests.Common;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public abstract class BaseUserPostServiceTest : BaseTest
    {
        public IUserPostService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var mapper = CommonTestMethods.GetMapper();
            var signInManager = CommonTestMethods.GetMockedSignInManager(userManager).Object;
            var service = new UserPostService(unitOfWork, userManager, signInManager, mapper);

            return service;
        }

        public IUserPostService GetService(
            IRedditCloneUnitOfWork unitOfWork, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager)
        {
            var mapper = CommonTestMethods.GetMapper();
            var service = new UserPostService(unitOfWork, userManager, signInManager, mapper);

            return service;
        }
    }
}
