using Microsoft.AspNetCore.Identity;
using RedditClone.Data.Interfaces;
using RedditClone.Models;
using RedditClone.Services.Tests.Common;
using RedditClone.Services.UserServices;
using RedditClone.Services.UserServices.Interfaces;

namespace RedditClone.Services.Tests.UserServicesTests.UserVotePostServiceTests
{
    public class BaseUserVoteServiceTest : BaseTest
    {
        internal IUserVotePostService GetService(User user, Post post, VotePost votePost)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Posts.Add(post);
            unitOfWork.VotePostRepository.Add(votePost);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork, user.Id);

            return service;
        }

        internal IUserVotePostService GetService(User user, Post post)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork, user.Id);

            return service;
        }

        internal IUserVotePostService GetService(User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var service = this.GetService(unitOfWork, user.Id);

            return service;
        }

        internal IUserVotePostService GetService(IRedditCloneUnitOfWork unitOfWork, string userId)
        {
            var mockedUserManager = CommonTestMethods.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserId(mockedUserManager, userId);
            var service = this.GetService(unitOfWork, mockedUserManager.Object);

            return service;
        }

        public IUserVotePostService GetService(IRedditCloneUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var service = new UserVotePostService(unitOfWork, userManager);
            return service;
        }

        internal VotePost CreateUpVotePost(User user, Post post)
        {
            var dbVotePost = this.CreateVotePost(user, post);
            dbVotePost.Value = 1;

            return dbVotePost;
        }

        internal VotePost CreateDownVotePost(User user, Post post)
        {
            var dbVotePost = this.CreateVotePost(user, post);
            dbVotePost.Value = -1;

            return dbVotePost;
        }

        private VotePost CreateVotePost(User user, Post post)
        {
            var dbVotePost = new VotePost()
            {
                User = user,
                Post = post
            };

            return dbVotePost;
        }

        internal void AddEnitytiesToUnitOfWork(IRedditCloneUnitOfWork unitOfWork, User user, Post post, VotePost votePost)
        {
            this.AddEnitytiesToUnitOfWork(unitOfWork, user, post);
            unitOfWork.VotePostRepository.Add(votePost);
            unitOfWork.Complete();
        }

        internal void AddEnitytiesToUnitOfWork(IRedditCloneUnitOfWork unitOfWork, User user, Post post)
        {
            unitOfWork.Users.Add(user);
            unitOfWork.Posts.Add(post);
            unitOfWork.Complete();
        }
    }
}
