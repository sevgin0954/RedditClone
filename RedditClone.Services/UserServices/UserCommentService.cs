using Microsoft.AspNetCore.Identity;
using RedditClone.Common.Validation;
using RedditClone.CustomMapper.Interfaces;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.CommentModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices
{
    public class UserCommentService : IUserCommentService
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly ICommentMapper commentMapper;

        public UserCommentService(
            IRedditCloneUnitOfWork unitOfWork, 
            UserManager<User> userManager,
            ICommentMapper commentMapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.commentMapper = commentMapper;
        }

        public async Task<bool> AddCommentToPostAsync(ClaimsPrincipal user, CommentBindingModel model)
        {
            var dbPost = await this.unitOfWork.Posts.GetByIdAsync(model.SourceId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbComment = this.commentMapper.MapComment(dbPost.Id, model.Description, dbUserId);

            this.unitOfWork.Comments.Add(dbComment);
            var rowsAffected = await unitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }

        public async Task<bool> AddResponseToCommentAsync(ClaimsPrincipal user, CommentBindingModel model)
        {
            var dbComment = await this.unitOfWork.Comments.GetByIdAsync(model.SourceId);
            if (dbComment == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbReply = this.commentMapper.MapComment(dbComment.PostId, model.Description, dbUserId);

            dbComment.Replies.Add(dbReply);
            var rowsAffected = await unitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }
    }
}
