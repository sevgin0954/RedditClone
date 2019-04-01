using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Services.UserServices.Interfaces;
using System.Collections.Generic;

namespace RedditClone.Services.UserServices
{
    public class UserPostService : IUserPostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;

        public UserPostService(IRedditCloneUnitOfWork redditCloneUnitOfWork, UserManager<User> userManager)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
        }

        public async Task<CreationPostBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId)
        {
            var dbUser = await this.userManager.GetUserAsync(user);

            var model = await this.MapCreatinPostModelAsync(dbUser.Id, subredditId);

            return model;
        }

        private async Task<CreationPostBindingModel> MapCreatinPostModelAsync(string userId, string subredditId)
        {
            var model = new CreationPostBindingModel
            {
                AuthorId = userId,
                SubredditId = subredditId
            };

            var createdByUserSubreddits = redditCloneUnitOfWork.Subreddits.Find(s => s.AuthorId == userId);
            var subscribedSubreddits = await redditCloneUnitOfWork.Subreddits.GetBySubcribedUserIdAsync(userId);

            model.CreatedSubreddits = this.MapSelectListItemsBySubreddits(createdByUserSubreddits);
            model.SubscribedSubreddits = this.MapSelectListItemsBySubreddits(subscribedSubreddits);

            return model;
        }

        private ICollection<SelectListItem> MapSelectListItemsBySubreddits(IEnumerable<Subreddit> subreddits)
        {
            var models = new List<SelectListItem>();

            foreach (var subreddit in subreddits)
            {
                var selectListItem = new SelectListItem()
                {
                    Text = subreddit.Name,
                    Value = subreddit.Id
                };

                models.Add(selectListItem);
            }

            return models;
        }
    }
}
