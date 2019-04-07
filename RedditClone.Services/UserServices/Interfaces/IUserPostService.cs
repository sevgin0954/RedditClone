﻿using Microsoft.AspNetCore.Http;
using RedditClone.Common.Enums;
using RedditClone.Models.WebModels.IndexModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices.Interfaces
{
    public interface IUserPostService
    {
        Task<PostCreationBindingModel> PrepareModelForCreatingAsync(ClaimsPrincipal user, string subredditId);

        Task<bool> CreatePostAsync(ClaimsPrincipal user, PostCreationBindingModel model);

        Task<IndexViewModel> GetPostsCustomizedByUser(
            ClaimsPrincipal user,
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies);

        Task<IndexViewModel> GetPostsForQuest(
            IRequestCookieCollection requestCookies,
            IResponseCookies responseCookies);

        void ChangePostSortType(IResponseCookies responseCookies, PostSortType postSortType);

        void ChangePostTimeFrame(IResponseCookies responseCookies, PostShowTimeFrame postShowTimeFrame);
    }
}
