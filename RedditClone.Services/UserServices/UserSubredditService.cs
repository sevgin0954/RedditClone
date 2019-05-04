using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RedditClone.Common.Validation;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices
{
    public class UserSubredditService : IUserSubredditService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserSubredditService(
            IRedditCloneUnitOfWork redditCloneUnitOfWork, 
            IMapper mapper,
            UserManager<User> userManager)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> CreateSubredditAsync(SubredditCreationBindingModel model, ClaimsPrincipal user)
        {
            var subredditsWithName = await this.redditCloneUnitOfWork.Subreddits.FindAsync(s => s.Name == model.Name);
            if (subredditsWithName.Count() > 0)
            {
                return false;
            }

            var dbSubreddit = this.mapper.Map<Subreddit>(model);
            var dbUserId = this.userManager.GetUserId(user);
            dbSubreddit.AuthorId = dbUserId;

            this.redditCloneUnitOfWork.Subreddits.Add(dbSubreddit);
            int rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }
    }
}
