using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using RedditClone.Models;
using RedditClone.Web.Mapping;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Tests.Common
{
    public abstract class CommonTestMethods
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = config.CreateMapper();

            return mapper;
        }

        public static void SetupMockedUserManagerGetUserAsync(Mock<UserManager<User>> mock, User user)
        {
            mock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(user));
        }

        public static void SetupMockedUserManagerGetUserId(Mock<UserManager<User>> mock, string userId)
        {
            mock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
        }
    }
}
