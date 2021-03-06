﻿using AutoMapper;
using RedditClone.Common.Constants;
using RedditClone.Data.Helpers;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using System.Linq;

namespace RedditClone.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Post, UserActionViewModel>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ActionInvokerId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.ActionInvokerUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => string.Concat(src.Title.Take(ModelsConstants.TitlePostPreviewLength))))
                .ForMember(dest => dest.SubredditName, opt => opt.MapFrom(src => src.Subreddit.Name))
                .ForMember(dest => dest.PostCreatorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.PostCreatorUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.DescriptionConcise,
                    opt => opt.MapFrom(src => string.Concat(src.Description.Take(ModelsConstants.DescriptionAccountPreviewLength))))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => ModelsConstants.ActionNamePost));

            this.CreateMap<Comment, UserActionViewModel>()
                .ForMember(dest => dest.ActionInvokerId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.ActionInvokerUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => string.Concat(src.Post.Title.Take(ModelsConstants.TitlePostPreviewLength))))
                .ForMember(dest => dest.SubredditId, opt => opt.MapFrom(src => src.Post.SubredditId))
                .ForMember(dest => dest.SubredditName, opt => opt.MapFrom(src => src.Post.Subreddit.Name))
                .ForMember(dest => dest.PostCreatorId, opt => opt.MapFrom(src => src.Post.AuthorId))
                .ForMember(dest => dest.PostCreatorUsername, opt => opt.MapFrom(src => src.Post.Author.UserName))
                .ForMember(dest => dest.DescriptionConcise,
                    opt => opt.MapFrom(src => string.Concat(src.Description.Take(ModelsConstants.DescriptionAccountPreviewLength))))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => ModelsConstants.ActionNameComment));

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<PostCreationBindingModel, Post>()
                .ForMember(dest => dest.SubredditId, opt => opt.MapFrom(src => src.SelectedSubredditId));

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<SubredditCreationBindingModel, Subreddit>();

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<Post, PostConciseViewModel>()
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => CountComments.Count(src.Comments)))
                .ForMember(dest => dest.VotesCount, opt => opt.MapFrom(src => src.UpVotesCount - src.DownVotesCount))
                .ForMember(dest => dest.DescriptionConcise, opt => opt.MapFrom(src => 
                    string.Concat(src.Description.Take(ModelsConstants.DescriptionIndexPreviewLength))));

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.VotesCount, opt => opt.MapFrom(src => src.UpVotesCount - src.DownVotesCount))
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.VotesCount, opt => opt.MapFrom(src => src.UpVotesCount - src.DownVotesCount));

            //------------------------------------------------------------------------------------------------------------

            this.CreateMap<Subreddit, SubredditConciseViewModel>()
                .ForMember(dest => dest.SubscribedUsersCount, opt => opt.MapFrom(src => src.SubscribedUsers.Count));
        }
    }
}
