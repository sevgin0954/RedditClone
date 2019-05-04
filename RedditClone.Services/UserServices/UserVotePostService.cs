using Microsoft.AspNetCore.Identity;
using RedditClone.Common.Validation;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.UserServices.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedditClone.Services.UserServices
{
    public class UserVotePostService : IUserVotePostService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly UserManager<User> userManager;

        public UserVotePostService(IRedditCloneUnitOfWork redditCloneUnitOfWork, UserManager<User> userManager)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.userManager = userManager;
        }

        public async Task<bool> AddDownVoteToPostAsync(string postId, ClaimsPrincipal user)
        {
            var dbPost = await this.redditCloneUnitOfWork.Posts.GetByIdAsync(postId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbVotePost = await this.redditCloneUnitOfWork.VotePostRepository
                .GetByUserIdAsync(dbUserId, postId);
            if (dbVotePost == null)
            {
                dbVotePost = this.CreatePost(dbUserId, postId);
                this.redditCloneUnitOfWork.VotePostRepository.Add(dbVotePost);
            }
            else if (dbVotePost.Value < 0)
            {
                return false;
            }
            else
            {
                dbPost.UpVotesCount--;
            }

            dbVotePost.Value = -1;
            dbPost.DownVotesCount++;

            var rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }

        public async Task<bool> AddUpVoteToPostAsync(string postId, ClaimsPrincipal user)
        {
            var dbPost = await this.redditCloneUnitOfWork.Posts.GetByIdAsync(postId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbVotePost = await this.redditCloneUnitOfWork.VotePostRepository
                .GetByUserIdAsync(dbUserId, postId);
            if (dbVotePost == null)
            {
                dbVotePost = this.CreatePost(dbUserId, postId);
                this.redditCloneUnitOfWork.VotePostRepository.Add(dbVotePost);
            }
            else if (dbVotePost.Value > 0)
            {
                return false;
            }
            else
            {
                dbPost.DownVotesCount--;
            }

            dbVotePost.Value = 1;
            dbPost.UpVotesCount++;

            var rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }

        private VotePost CreatePost(string userId, string postId)
        {
            var dbVotePost = new VotePost()
            {
                UserId = userId,
                PostId = postId
            };

            return dbVotePost;
        }

        public async Task<bool> RemoveDownVoteToPostAsync(string postId, ClaimsPrincipal user)
        {
            var dbPost = await this.redditCloneUnitOfWork.Posts.GetByIdAsync(postId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbVotePost = await this.redditCloneUnitOfWork.VotePostRepository
                .GetByUserIdAsync(dbUserId, postId);
            if (this.IsDownVoteValid(dbVotePost) == false)
            {
                return false;
            }

            dbPost.DownVotesCount--;
            this.redditCloneUnitOfWork.VotePostRepository.Remove(dbVotePost);
            var rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }

        public async Task<bool> RemoveUpVoteToPostAsync(string postId, ClaimsPrincipal user)
        {
            var dbPost = await this.redditCloneUnitOfWork.Posts.GetByIdAsync(postId);
            if (dbPost == null)
            {
                return false;
            }

            var dbUserId = this.userManager.GetUserId(user);
            var dbVotePost = await this.redditCloneUnitOfWork.VotePostRepository
                .GetByUserIdAsync(dbUserId, postId);
            if (this.IsUpVoteValid(dbVotePost) == false)
            {
                return false;
            }

            dbPost.UpVotesCount--;
            this.redditCloneUnitOfWork.VotePostRepository.Remove(dbVotePost);
            var rowsAffected = await this.redditCloneUnitOfWork.CompleteAsync();
            return UnitOfWorkValidator.IsUnitOfWorkCompletedSuccessfully(rowsAffected);
        }

        private bool IsUpVoteValid(VotePost votePost)
        {
            if (votePost == null)
            {
                return false;
            }
            if (votePost.Value <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsDownVoteValid(VotePost votePost)
        {
            if (votePost == null)
            {
                return false;
            }
            if (votePost.Value >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
