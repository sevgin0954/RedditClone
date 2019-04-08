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
using AutoMapper;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Common.Enums;
using System;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Factories.PostsFactories;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Data.Orders.PostOrders.Interfaces;
using RedditClone.Data.Orders.PostOrders;
using Microsoft.AspNetCore.Http;
using RedditClone.Common.Helpers;
using System.Linq;

namespace RedditClone.Services.UserServices
{
    public class UserPostService : IUserPostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public UserPostService(
            IRedditCloneUnitOfWork redditCloneUnitOfWork, 
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<PostCreationBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId)
        {
            var dbUserId = this.userManager.GetUserId(user);
            var dbSubreddit = await this.redditCloneUnitOfWork.Subreddits.GetByIdAsync(subredditId);

            var dbSubredditId = dbSubreddit?.Id;
            var model = await this.MapCreationPostBindingModelAsync(dbUserId, dbSubredditId);

            return model;
        }

        public async Task<bool> CreatePostAsync(ClaimsPrincipal user, PostCreationBindingModel model)
        {
            var dbSubredditWithId = await this.redditCloneUnitOfWork.Subreddits
                .GetByIdAsync(model.SelectedSubredditId);
            var isSubredditWithIdExist = dbSubredditWithId != null;

            var result = false;

            if (isSubredditWithIdExist)
            {
                var dbUserId = this.userManager.GetUserId(user);
                var dbPost = this.mapper.Map<Post>(model);
                dbPost.AuthorId = dbUserId;

                redditCloneUnitOfWork.Posts.Add(dbPost);
                var rowsAffected = await redditCloneUnitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<IndexViewModel> GetOrderedPostsAsync(
            ClaimsPrincipal user,
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var postSortTypeKey = WebConstants.CookieKeyPostSortType;
            var postTimeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var postSortTypeValue = requestCookies[postSortTypeKey];
            var postTimeFrameValue = requestCookies[postTimeFrameKey];

            var postSortType = PostSortType.Best;
            var postShowTimeFrame = PostShowTimeFrame.PastDay;

            if (Enum.TryParse(postSortTypeValue, out postSortType) == false)
            {
                CookiesHelper.SetDefaultPostSortTypeCookie(responseCookies);
                postSortType = PostSortType.Best;
            }
            if (Enum.TryParse(postTimeFrameValue, out postShowTimeFrame) == false)
            {
                CookiesHelper.SetDefaultPostShowTimeFrameCookie(responseCookies);
                postShowTimeFrame = PostShowTimeFrame.PastDay;
            }

            var timeFrame = TimeFrameFactory.GetTimeFrame(postShowTimeFrame);
            var sortPostsStrategy = SortPostsFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            IEnumerable<Post> dbPosts = new List<Post>();

            var isSignIn = this.signInManager.IsSignedIn(user);
            if (isSignIn)
            {
                var dbUserId = this.userManager.GetUserId(user);
                dbPosts = await sortPostsStrategy.GetSortedPostsByUserAsync(dbUserId);
                if (dbPosts.Count() == 0)
                {
                    dbPosts = await sortPostsStrategy.GetSortedPostsAsync();
                }
            }
            else
            {
                dbPosts = await sortPostsStrategy.GetSortedPostsAsync();
            }

            var isHaveTimeFrame = CheckIsHaveTimeFrame(sortPostsStrategy);
            if (isHaveTimeFrame)
            {
                var model = this.MapIndexModelWithTimeFrame(dbPosts, postSortType, postShowTimeFrame);
                return model;
            }
            else
            {
                CookiesHelper.SetDefaultPostShowTimeFrameCookie(responseCookies);
                var model = this.MapIndexModel(dbPosts, postSortType);
                return model;
            }
        }

        public void ChangePostSortType(IResponseCookies responseCookies, PostSortType postSortType)
        {
            var sortTypeKey = WebConstants.CookieKeyPostSortType;
            var sortTypeValue = postSortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        public void ChangePostTimeFrame(IResponseCookies responseCookies, PostShowTimeFrame postShowTimeFrame)
        {
            var timeFrameKey = WebConstants.CookieKeyPostShowTimeFrame;
            var timeFrameValue = postShowTimeFrame.ToString();

            responseCookies.Append(timeFrameKey, timeFrameValue);
        }

        private bool CheckIsHaveTimeFrame(ISortPostsStrategy sortPostsStrategy)
        {
            var isHaveTimeFrame = false;

            if (sortPostsStrategy is BaseTimeDependentPostSortingStrategy)
            {
                isHaveTimeFrame = true;
            }

            return isHaveTimeFrame;
        }

        private async Task<PostCreationBindingModel> MapCreationPostBindingModelAsync(string userId, string subredditId)
        {
            var model = new PostCreationBindingModel
            {
                SelectedSubredditId = subredditId
            };

            await this.MapPostModelSubredditsAsync(model, userId, subredditId);

            return model;
        }

        private async Task MapPostModelSubredditsAsync(PostCreationBindingModel model, string userId, string subredditId)
        {
            var createdSubreddits = await redditCloneUnitOfWork.Subreddits
                .GetByAuthorAsync(userId);
            var subscribedSubreddits = await redditCloneUnitOfWork.Subreddits
                .GetBySubcribedUserAsync(userId);

            var createdSubredditGroupName = ModelsConstants.SelectListGroupNameCreatedSubreddits;
            var createdSubredditsSelectListItems
                = this.MapSelectListItemsBySubreddits(createdSubreddits, createdSubredditGroupName, subredditId);

            var subscribedSubredditGroupName = ModelsConstants.SelectListGroupNameSubscribedSubreddits;
            var subscribedSubredditsSelectListItem
                = this.MapSelectListItemsBySubreddits(subscribedSubreddits, subscribedSubredditGroupName, subredditId);

            model.Subreddits.AddRange(createdSubredditsSelectListItems);
            model.Subreddits.AddRange(subscribedSubredditsSelectListItem);
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

            if (subreddits.Count() == 0)
            {
                var text = ModelsConstants.SelectListItemNameEmpty;
                var initialCreatedItem = this.CreateEmptySelectListItem(groupName, text);
                models.Add(initialCreatedItem);
            }

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

        private IndexViewModel MapIndexModelWithTimeFrame(
            IEnumerable<Post> posts,
            PostSortType selectedSortType,
            PostShowTimeFrame selectedTimeFrame)
        {
            var postModels = this.mapper.Map<IEnumerable<PostConciseViewModel>>(posts);

            var model = new IndexViewModel
            {
                Posts = postModels,
                PostSortType = selectedSortType,
                PostShowTimeFrame = selectedTimeFrame
            };

            return model;
        }

        private IndexViewModel MapIndexModel(
            IEnumerable<Post> posts,
            PostSortType selectedSortType)
        {
            var postModels = this.mapper.Map<IEnumerable<PostConciseViewModel>>(posts);

            var model = new IndexViewModel
            {
                Posts = postModels,
                PostSortType = selectedSortType
            };

            return model;
        }
    }
}
