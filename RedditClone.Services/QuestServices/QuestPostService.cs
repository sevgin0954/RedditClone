using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Helpers;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestPostService : IQuestPostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly IMapper mapper;
        private readonly ICookieService cookieService;

        public QuestPostService(IRedditCloneUnitOfWork redditCloneUnitOfWork, IMapper mapper, ICookieService cookieService)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.mapper = mapper;
            this.cookieService = cookieService;
        }

        public async Task<PostsViewModel> GetOrderedPostsAsync(
            IRequestCookieCollection requestCookies)
        {
            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postTimeFrameType = this.cookieService.GetPostTimeFrameTypeFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postTimeFrameType);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetAllSortedByAsync(sortPostsStrategy);

            var model = this.MapIndexModel(dbPosts, postSortType, sortPostsStrategy, postTimeFrameType);
            return model;
        }

        public async Task<PostsViewModel> GetOrderedPostsBySubredditAsync(
            string subredditId, 
            IRequestCookieCollection requestCookies)
        {
            var dbSubreddit = await this.redditCloneUnitOfWork.Subreddits
                .GetByIdAsync(subredditId);
            if (dbSubreddit == null)
            {
                return null;
            }

            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postTimeFrameType = this.cookieService.GetPostTimeFrameTypeFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postTimeFrameType);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetBySubredditSortedBy(subredditId, sortPostsStrategy);

            var model = this.MapIndexModel(dbPosts, postSortType, sortPostsStrategy, postTimeFrameType);
            return model;
        }

        public async Task<PostsViewModel> GetOrderedPostsByKeyWordsAsync(
            string[] keyWords,
            PostSortType sortType, 
            TimeFrameType timeFrameType)
        {
            var timeFrame = TimeFrameFactory.GetTimeFrame(timeFrameType);
            var sortStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, sortType);

            var filteredPosts = await this.redditCloneUnitOfWork.Posts
                .GetByKeyWordsSortedByAsync(keyWords, sortStrategy);

            var model = this.MapIndexModel(filteredPosts, sortType, sortStrategy, timeFrameType);

            return model;
        }

        private PostsViewModel MapIndexModel(
            IEnumerable<Post> posts,
            PostSortType selectedPostSortType,
            ISortPostsStrategy sortStrategy,
            TimeFrameType selectedTimeFrameType)
        {
            var postModels = this.mapper.Map<IEnumerable<PostConciseViewModel>>(posts);
            var model = new PostsViewModel
            {
                Posts = postModels,
                PostSortType = selectedPostSortType
            };

            var isHaveTimeFrame = CheckIsSortStrategyHaveTimeFrame(sortStrategy);
            if (isHaveTimeFrame)
            {
                model.PostTimeFrameType = selectedTimeFrameType;
            }

            return model;
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

        public async Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            string postId, 
            IRequestCookieCollection requestCookies)
        {
            var dbPost = await redditCloneUnitOfWork.Posts.GetByIdWithIncludedAllProperties(postId);
            if (dbPost == null)
            {
                return null;
            }

            var commentSortType = this.cookieService.GetCommentSortTypeFromCookieOrDefault(requestCookies);

            var sortTypeStrategy = SortCommentStrategyFactory.GetSortPostsStrategy(redditCloneUnitOfWork, commentSortType);
            var sortedComments = await this.redditCloneUnitOfWork.Comments
                .GetByPostSortedByAsync(postId, sortTypeStrategy);

            var model = this.MapPostModel(dbPost, commentSortType, sortedComments);

            return model;
        }

        private PostViewModel MapPostModel(Post post, CommentSortType sortType, IEnumerable<Comment> comments)
        {
            int commentCount = CountComments.Count(comments);

            var model = this.mapper.Map<PostViewModel>(post);
            model.SelectedCommentSortType = sortType;
            model.CommentsCount = commentCount;
            model.Comments = this.mapper.Map<IEnumerable<CommentViewModel>>(comments);

            return model;
        }
    }
}
