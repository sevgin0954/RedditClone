using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.CustomMapper.Interfaces;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Interfaces;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestPostService : IQuestPostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly ICookieService cookieService;
        private readonly IPostMapper postMapper;

        public QuestPostService(
            IRedditCloneUnitOfWork redditCloneUnitOfWork,
            ICookieService cookieService,
            IPostMapper postMapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.cookieService = cookieService;
            this.postMapper = postMapper;
        }

        public async Task<PostsViewModel> GetOrderedPostsAsync(IRequestCookieCollection requestCookies)
        {
            var postSortType = this.cookieService.GetPostSortTypeFromCookieOrDefault(requestCookies);
            var postTimeFrameType = this.cookieService.GetPostTimeFrameTypeFromCookieOrDefault(requestCookies);

            var timeFrame = TimeFrameFactory.GetTimeFrame(postTimeFrameType);
            var sortPostsStrategy = SortPostsStartegyFactory
                .GetSortPostsStrategy(this.redditCloneUnitOfWork, timeFrame, postSortType);

            var dbPosts = await this.redditCloneUnitOfWork.Posts
                .GetAllSortedByAsync(sortPostsStrategy);

            var model = 
                this.postMapper.MapPostsViewModelForQuest(dbPosts, postSortType, sortPostsStrategy, postTimeFrameType);
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

            var model = this.postMapper.MapPostsViewModelForQuest(
                dbPosts, postSortType, sortPostsStrategy, postTimeFrameType);
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

            var model = this.postMapper.MapPostsViewModelForQuest(filteredPosts, sortType, sortStrategy, timeFrameType);

            return model;
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

            var model = this.postMapper.MapPostViewModelForQuest(dbPost, commentSortType, sortedComments);

            return model;
        }
    }
}
