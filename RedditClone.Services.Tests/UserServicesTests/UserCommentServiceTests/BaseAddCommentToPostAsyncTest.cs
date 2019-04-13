using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.Tests.UserServicesTests.UserCommentServiceTests
{
    public class BaseAddCommentToPostAsyncTest : BaseTest
    {
        public IUserCommentService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var service = new UserCommentService(unitOfWork, userManager);

            return service;
        }
    }
}
