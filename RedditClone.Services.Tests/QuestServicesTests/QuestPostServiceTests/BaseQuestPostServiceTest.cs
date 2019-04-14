﻿using Moq;
using RedditClone.Data.Interfaces;
using RedditClone.Services.QuestServices;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.Tests.Common;

namespace RedditClone.Services.Tests.QuestServicesTests.QuestPostServiceTests
{
    public class BaseQuestPostServiceTest : BaseTest
    {
        public IQuestPostService GetService(IRedditCloneUnitOfWork unitOfWork)
        {
            var mapper = CommonTestMethods.GetMapper();
            var cookieSerive = new Mock<ICookieService>().Object;
            var service = new QuestPostService(unitOfWork, mapper, cookieSerive);

            return service;
        }
    }
}
