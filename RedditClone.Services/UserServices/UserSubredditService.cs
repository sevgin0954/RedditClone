using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Security.Claims;

namespace RedditClone.Services.UserServices
{
    public class UserSubredditService : IUserSubredditService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;

        public UserSubredditService(IRedditCloneUnitOfWork redditCloneUnitOfWork, UserManager<User> userManager)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
        }
        
        public SubredditCreationBindingModel PrepareModelForCreating(ClaimsPrincipal user)
        {
            var dbUserId = this.userManager.GetUserId(user);
            var model = new SubredditCreationBindingModel
            {
                AuthorId = dbUserId
            };

            return model;
        }
    }
}
