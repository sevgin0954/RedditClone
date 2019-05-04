using Microsoft.AspNetCore.Identity;
using Moq;
using RedditClone.CustomMapper;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.Tests.Common;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.Tests.UserServicesTests.UserPostServiceTests
{
    public abstract class BaseUserPostServiceTest : BaseTest
    {
        public IUserPostService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var cookieSerive = new Mock<ICookieService>().Object;
            var autoMapper = CommonTestMethods.GetAutoMapper();
            var postMapper = new PostMapper(autoMapper);
            var service = new UserPostService(unitOfWork, userManager, cookieSerive, postMapper);

            return service;
        }
    }
}
