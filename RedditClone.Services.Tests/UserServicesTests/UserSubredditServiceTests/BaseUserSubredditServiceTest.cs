﻿using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.Tests.Common;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.Tests.UserServicesTests.UserSubredditServiceTests
{
    public abstract class BaseUserSubredditServiceTest : BaseTest
    {
        public IUserSubredditService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var mapper = CommonTestMethods.GetAutoMapper();
            var service = new UserSubredditService(unitOfWork, mapper, userManager);

            return service;
        }
    }
}
