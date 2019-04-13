using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedditClone.Common.Constants;
using RedditClone.Common.Enums;
using RedditClone.Common.Helpers;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestPostService : IQuestPostService
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public QuestPostService(IRedditCloneUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PostViewModel> GetPostWithOrderedCommentsAsync(
            string postId, 
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies)
        {
            var dbPost = await unitOfWork.Posts.GetByIdWithIncludedAllProperties(postId);
            if (dbPost == null)
            {
                return null;
            }

            var commentSortTypeKey = WebConstants.CookieKeyCommentSortType;
            var commentSortTypeValue = requestCookies[commentSortTypeKey];

            var commentSortType = SortType.Best;
            if (Enum.TryParse(commentSortTypeValue, out commentSortType) == false)
            {
                CookiesHelper.SetDefaultCommentSortTypeCookie(responseCookies);
                commentSortType = SortType.Best;
            }

            var sortTypeStrategy = SortCommentStrategyFactory.GetSortPostsStrategy(unitOfWork, commentSortType);
            var sortedComments = await sortTypeStrategy.GetSortedCommentsAsync(postId);

            var model = this.MapPostModel(dbPost, commentSortType, sortedComments);

            return model;
        }

        public void ChangeCommentSortType(IResponseCookies responseCookies, SortType sortType)
        {
            var sortTypeKey = WebConstants.CookieKeyCommentSortType;
            var sortTypeValue = sortType.ToString();

            responseCookies.Append(sortTypeKey, sortTypeValue);
        }

        private PostViewModel MapPostModel(Post post, SortType sortType, IEnumerable<Comment> comments)
        {
            int commentCount = this.CountComments(comments);

            var model = this.mapper.Map<PostViewModel>(post);
            model.SelectedSortType = sortType;
            model.CommentsCount = commentCount;
            model.Comments = this.mapper.Map<IEnumerable<CommentViewModel>>(comments);

            return model;
        }

        private int CountComments(IEnumerable<Comment> comments)
        {
            if (comments.Count() == 0)
            {
                return 0;
            }

            int totalCommentsCount = comments.Count();

            foreach (var comment in comments)
            {
                int RepliesCommentsCount = this.CountComments(comment.Replies);
                totalCommentsCount += RepliesCommentsCount;
            }

            return totalCommentsCount;
        }
    }
}
