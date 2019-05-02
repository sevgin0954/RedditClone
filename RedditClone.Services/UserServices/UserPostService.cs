using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using AutoMapper;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Data.Factories.TimeFactories;
using Microsoft.AspNetCore.Http;
using System.Linq;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.CustomMapper.Interfaces;

namespace RedditClone.Services.UserServices
{
    public class UserPostService : IUserPostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;
        private readonly ICookieService cookieService;
        private readonly IMapper mapper;
        private readonly IPostMapper postMapper;

        public UserPostService(
            IRedditCloneUnitOfWork redditCloneUnitOfWork, 
            UserManager<User> userManager,
            ICookieService cookieService,
            IMapper mapper,
            IPostMapper postMapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
            this.cookieService = cookieService;
            this.mapper = mapper;
            this.postMapper = postMapper;
        }

        public async Task<PostCreationBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId)
        {
            var dbUserId = this.userManager.GetUserId(user);
            var dbSubreddit = await this.redditCloneUnitOfWork.Subreddits.GetByIdAsync(subredditId);

            // TODO: Refactor (?)
            var dbSubredditId = dbSubreddit?.Id;

            var createdSubreddits = await redditCloneUnitOfWork.Subreddits
                .GetByAuthorAsync(dbUserId);
            var subscribedSubreddits = await redditCloneUnitOfWork.Subreddits
                .GetBySubcribedUserAsync(dbUserId);

            var model = this.postMapper.MapPostCreationBindingModel(
                createdSubreddits,
                subscribedSubreddits,
                dbSubredditId);

            return model;
        }

        public async Task<bool> CreatePostAsync(ClaimsPrincipal user, PostCreationBindingModel model)
        {
            var dbSubredditWithId = await this.redditCloneUnitOfWork.Subreddits
                .GetByIdAsync(model.SelectedSubredditId);

            var result = false;

            var isSubredditWithIdExist = dbSubredditWithId != null;
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

        public async Task<PostsViewModel> GetOrderedPostsAsync(
            ClaimsPrincipal user,
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postTimeFrameType = this.cookieService.GetPostTimeFrameTypeFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postTimeFrameType);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbUserId = this.userManager.GetUserId(user);
            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetBySubcribedUserSortedByAsync(dbUserId, sortPostsStrategy);
            if (dbPosts.Count() == 0)
            {
                dbPosts = await this.redditCloneUnitOfWork.Posts.GetAllSortedByAsync(sortPostsStrategy);
            }

            var model = this.postMapper
                .MapPostsViewModel(dbPosts, dbUserId, postSortType, sortPostsStrategy, postTimeFrameType);
            return model;
        }
    }
}
