using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public SubredditCreationBindingModel PrepareModelForCreating()
        {
            var model = new SubredditCreationBindingModel();

            return model;
        }

        public async Task<bool> CreateSubredditAsync(SubredditCreationBindingModel model, ClaimsPrincipal user)
        {
            var subredditsWithName = this.redditCloneUnitOfWork.Subreddits.Find(s => s.Name == model.Name);
            var isSubredditWithNameExist = subredditsWithName.Count() > 0;

            var result = false;

            if (isSubredditWithNameExist == false)
            {
                var dbSubreddit = this.mapper.Map<Subreddit>(model);
                dbSubreddit.AuthorId = this.userManager.GetUserId(user); ;
                this.redditCloneUnitOfWork.Subreddits.Add(dbSubreddit);
                int rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
