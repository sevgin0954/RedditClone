﻿using AutoMapper;
using RedditClone.Common.Constants;
using RedditClone.Models;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using System.Linq;

namespace RedditClone.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Post, UserIndexViewModel>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ActionInvokerId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.ActionInvokerUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.SubredditName, opt => opt.MapFrom(src => src.Subreddit.Name))
                .ForMember(dest => dest.PostCreatorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.PostCreatorUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.DescriptionConcise,
                    opt => opt.MapFrom(src => string.Concat(src.Description.Take(ModelsConstants.DescriptionPreviewLength))))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => ModelsConstants.ActionNamePost));

            this.CreateMap<Comment, UserIndexViewModel>()
                .ForMember(dest => dest.ActionInvokerId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.ActionInvokerUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post.Title))
                .ForMember(dest => dest.SubredditId, opt => opt.MapFrom(src => src.Post.SubredditId))
                .ForMember(dest => dest.SubredditName, opt => opt.MapFrom(src => src.Post.Subreddit.Name))
                .ForMember(dest => dest.PostCreatorId, opt => opt.MapFrom(src => src.Post.AuthorId))
                .ForMember(dest => dest.PostCreatorUsername, opt => opt.MapFrom(src => src.Post.Author.UserName))
                .ForMember(dest => dest.DescriptionConcise,
                    opt => opt.MapFrom(src => string.Concat(src.Description.Take(ModelsConstants.DescriptionPreviewLength))))
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => ModelsConstants.ActionNameComment));
        }
    }
}