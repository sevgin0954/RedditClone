using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RedditClone.Data.Interfaces;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.UserServices
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly IMapper mapper;

        public UserAccountService(IRedditCloneUnitOfWork redditCloneUnitOfWork, IMapper mapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserIndexViewModel>> PrepareIndexModelAsync(string userId)
        {
            var dbUser = await this.redditCloneUnitOfWork.Users.GetByIdAsync(userId);
            if (dbUser == null)
            {
                return null;
            }

            var dbAllPostsFromUser = await this.redditCloneUnitOfWork.Posts.GetWithSubredditByAuthorAsync(dbUser.Id);
            var dbAllCommentsFromUser = await this.redditCloneUnitOfWork.Comments.GetWithPostByUserIdAsync(dbUser.Id);

            var models = new List<UserIndexViewModel>();
            models.AddRange(this.mapper.Map<IEnumerable<UserIndexViewModel>>(dbAllPostsFromUser));
            models.AddRange(this.mapper.Map<IEnumerable<UserIndexViewModel>>(dbAllCommentsFromUser));
            
            return models.OrderByDescending(m => m.PostDate);
        }
    }
}
