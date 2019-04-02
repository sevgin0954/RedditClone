using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Services.UserServices.Interfaces;
using System.Collections.Generic;
using RedditClone.Common.Constants;

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

        public async Task<PostCreationBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId)
        {
            var dbUserId = this.userManager.GetUserId(user);
            var dbSubreddit = await this.redditCloneUnitOfWork.Subreddits.GetByIdAsync(subredditId);

            var dbSubredditId = dbSubreddit?.Id;
            var model = await this.MapCreationPostBindingModelAsync(dbUserId, dbSubredditId);

            return model;
        }

        private async Task<PostCreationBindingModel> MapCreationPostBindingModelAsync(string userId, string subredditId)
        {
            var model = new PostCreationBindingModel
            {
                SelectedSubredditId = subredditId
            };

            var createdSubreddits = redditCloneUnitOfWork.Subreddits.Find(s => s.AuthorId == userId);
            var subscribedSubreddits = await redditCloneUnitOfWork.Subreddits.GetBySubcribedUserIdAsync(userId);

            var createdGroupName = ModelsConstants.SelectListGroupNameCreatedSubreddits;
            var subscribedGroupName = ModelsConstants.SelectListGroupNameSubscribedSubreddits;

            var createdSubredditsSelectListItems 
                = this.MapSelectListItemsBySubreddits(createdSubreddits, createdGroupName, subredditId);
            if (createdSubredditsSelectListItems.Count == 0)
            {
                var text = ModelsConstants.SelectListItemNameEmpty;
                var initialCreatedItem = this.CreateEmptySelectListItem(createdGroupName, text);
                createdSubredditsSelectListItems.Add(initialCreatedItem);
            }

            var subscribedSubredditsSelectListItem 
                = this.MapSelectListItemsBySubreddits(subscribedSubreddits, subscribedGroupName, subredditId);
            if (subscribedSubredditsSelectListItem.Count == 0)
            {
                var text = ModelsConstants.SelectListItemNameEmpty;
                var initialCreatedItem = this.CreateEmptySelectListItem(subscribedGroupName, text);
                subscribedSubredditsSelectListItem.Add(initialCreatedItem);
            }
            
            model.Subreddits.AddRange(createdSubredditsSelectListItems);
            model.Subreddits.AddRange(subscribedSubredditsSelectListItem);

            return model;
        }

        private ICollection<SelectListItem> MapSelectListItemsBySubreddits(
            IEnumerable<Subreddit> subreddits, 
            string groupName,
            string selectedSubredditId)
        {
            var models = new List<SelectListItem>();
            var group = new SelectListGroup
            {
                Name = groupName
            };

            foreach (var subreddit in subreddits)
            {
                var selectListItem = this.MapSelectListItemBySubreddit(subreddit, group, selectedSubredditId);
                models.Add(selectListItem);
            }

            return models;
        }

        private SelectListItem MapSelectListItemBySubreddit(
            Subreddit subreddit, 
            SelectListGroup group, 
            string selectedSubredditId)
        {
            bool isSelected = false;
            if (subreddit.Id == selectedSubredditId)
            {
                isSelected = true;
            }

            var selectListItem = new SelectListItem()
            {
                Text = subreddit.Name,
                Value = subreddit.Id,
                Selected = isSelected,
                Group = group
            };

            return selectListItem;
        }

        private SelectListItem CreateEmptySelectListItem(string groupName, string text)
        {
            var group = new SelectListGroup
            {
                Name = groupName
            };
            var selectListItem = new SelectListItem()
            {
                Disabled = true,
                Group = group,
                Text = text
            };

            return selectListItem;
        }
    }
}
