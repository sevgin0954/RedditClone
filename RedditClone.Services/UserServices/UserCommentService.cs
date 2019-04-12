using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices
{
    public class UserCommentService : IUserCommentService
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public UserCommentService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<bool> AddCommentToPostAsync(ClaimsPrincipal user, CommentBindingModel model)
        {
            var dbPost = await this.unitOfWork.Posts.GetByIdAsync(model.SourceId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);

            var dbComment = new Comment()
            {
                PostId = model.SourceId,
                AuthorId = dbUserId,
                Description = model.Description,
                PostDate = DateTime.UtcNow
            };

            this.unitOfWork.Comments.Add(dbComment);
            var rowsAffected = await unitOfWork.CompleteAsync();
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
