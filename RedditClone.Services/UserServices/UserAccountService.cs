using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.UserServices
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserAccountService(IRedditCloneUnitOfWork redditCloneUnitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserIndexViewModel>> PrepareIndexModelAsync(ClaimsPrincipal user)
        {
            var dbUser = await this.userManager.GetUserAsync(user);
            var dbAllPostsFromUser = await this.redditCloneUnitOfWork.Posts.GetAllWithSubredditByUserIdAsync(dbUser.Id);
            var dbAllCommentsFromUser = await this.redditCloneUnitOfWork.Comments.GetAllWithPostByUserIdAsync(dbUser.Id);

            var models = new List<UserIndexViewModel>();
            models.AddRange(this.mapper.Map<IEnumerable<UserIndexViewModel>>(dbAllPostsFromUser));
            models.AddRange(this.mapper.Map<IEnumerable<UserIndexViewModel>>(dbAllCommentsFromUser));
            
            return models.OrderByDescending(m => m.PostDate);
        }
    }
}
