using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.CustomMapper.Interfaces;
using RedditClone.Data.SortStrategies;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RedditClone.CustomMapper
{
    public class PostMapper : IPostMapper
    {
        private readonly IMapper mapper;

        public PostMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public PostsViewModel MapPostsViewModel(
            IEnumerable<Post> posts,
            string userId,
            PostSortType selectedSortType,
            ISortPostsStrategy sortStrategy,
            TimeFrameType selectedTimeFrameType)
        {
            var postModels = this.MapPostConciseViewModels(posts, userId);
            var model = new PostsViewModel
            {
                Posts = postModels,
                PostSortType = selectedSortType
            };

            var isHaveTimeFrame = CheckIsSortStrategyHaveTimeFrame(sortStrategy);
            if (isHaveTimeFrame)
            {
                model.PostTimeFrameType = selectedTimeFrameType;
            }

            return model;
        }

        private IEnumerable<PostConciseViewModel> MapPostConciseViewModels(IEnumerable<Post> posts, string userId)
        {
            var models = new List<PostConciseViewModel>();
            
            foreach (var post in posts)
            {
                var model = this.mapper.Map<PostConciseViewModel>(post);
                model.UserVoteValue = this.GetUserVoteValueOrDefault(post.Votes, userId);
                models.Add(model);
            }

            return models;
        }

        private int GetUserVoteValueOrDefault(IEnumerable<VotePost> votes, string userId)
        {
            var userVote = votes.Where(v => v.UserId == userId).FirstOrDefault();
            if (userVote != null)
            {
                return userVote.Value;
            }
            else
            {
                return 0;
            }
        }

        private bool CheckIsSortStrategyHaveTimeFrame(ISortPostsStrategy sortPostsStrategy)
        {
            var isHaveTimeFrame = false;

            if (sortPostsStrategy is BaseTimeDependentPostSortingStrategy)
            {
                isHaveTimeFrame = true;
            }

            return isHaveTimeFrame;
        }

        public PostCreationBindingModel MapPostCreationBindingModel(
            IEnumerable<Subreddit> createdSubreddits,
            IEnumerable<Subreddit> subscribedSubreddits,
            string selectedSubredditId)
        {
            var model = new PostCreationBindingModel();

            var createdSubredditGroupName = ModelsConstants.SelectListGroupNameCreatedSubreddits;
            var createdSubredditsSelectListItems
                = this.MapSelectListItemsBySubreddits(createdSubreddits, createdSubredditGroupName, selectedSubredditId);

            var subscribedSubredditGroupName = ModelsConstants.SelectListGroupNameSubscribedSubreddits;
            var subscribedSubredditsSelectListItem
                = this.MapSelectListItemsBySubreddits(subscribedSubreddits, subscribedSubredditGroupName, selectedSubredditId);

            model.Subreddits.AddRange(createdSubredditsSelectListItems);
            model.Subreddits.AddRange(subscribedSubredditsSelectListItem);

            model.SelectedSubredditId = selectedSubredditId;

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
    }
}
