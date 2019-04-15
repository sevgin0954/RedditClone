using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Helpers;
using RedditClone.Data.Interfaces;
using RedditClone.Data.SortStrategies.PostOrders;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
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

        public async Task<IndexViewModel> GetOrderedPostsAsync(
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postShowTimeFrame = this.cookieService.GetPostShowTimeFrameFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postShowTimeFrame);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetAllSortedByAsync(sortPostsStrategy);

            var model = this.MapIndexModel(dbPosts, postSortType, sortPostsStrategy, postShowTimeFrame);
            return model;
        }

        public async Task<IndexViewModel> GetOrderedPostsBySubredditAsync(
            string subredditId, 
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var dbSubreddit = await this.redditCloneUnitOfWork.Subreddits
                .GetByIdAsync(subredditId);
            if (dbSubreddit == null)
            {
                return null;
            }

            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postShowTimeFrame = this.cookieService.GetPostShowTimeFrameFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postShowTimeFrame);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetBySubredditSortedBy(subredditId, sortPostsStrategy);

            var model = this.MapIndexModel(dbPosts, postSortType, sortPostsStrategy, postShowTimeFrame);
            return model;
        }

        private IndexViewModel MapIndexModel(
            IEnumerable<Post> posts,
            SortType selectedSortType,
            ISortPostsStrategy sortStrategy,
            PostShowTimeFrame selectedTimeFrame)
        {
            var postModels = this.mapper.Map<IEnumerable<PostConciseViewModel>>(posts);
            var model = new IndexViewModel
            {
                Posts = postModels,
                PostSortType = selectedSortType
            };

            var isHaveTimeFrame = CheckIsSortStrategyHaveTimeFrame(sortStrategy);
            if (isHaveTimeFrame)
            {
                model.PostShowTimeFrame = selectedTimeFrame;
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
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var dbPost = await redditCloneUnitOfWork.Posts.GetByIdWithIncludedAllProperties(postId);
            if (dbPost == null)
            {
                return null;
            }

            var commentSortType = this.cookieService.GetCommentSortTypeFromCookieOrDefault(requestCookies);

            var sortTypeStrategy = SortCommentStrategyFactory.GetSortPostsStrategy(redditCloneUnitOfWork, commentSortType);
            var sortedComments = await sortTypeStrategy.GetSortedCommentsAsync(postId);

            var model = this.MapPostModel(dbPost, commentSortType, sortedComments);

            return model;
        }

        private PostViewModel MapPostModel(Post post, SortType sortType, IEnumerable<Comment> comments)
        {
            int commentCount = CountComments.Count(comments);

            var model = this.mapper.Map<PostViewModel>(post);
            model.SelectedSortType = sortType;
            model.CommentsCount = commentCount;
            model.Comments = this.mapper.Map<IEnumerable<CommentViewModel>>(comments);

            return model;
        }
    }
}
