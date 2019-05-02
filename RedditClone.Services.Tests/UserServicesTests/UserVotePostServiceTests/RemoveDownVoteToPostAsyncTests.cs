using Moq;
using RedditClone.Models;
using RedditClone.Services.UserServices.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserVotePostServiceTests
{
    public class RemoveDownVoteToPostAsyncTests : BaseUserVoteServiceTest
    {
        [Fact]
        public async Task WithIncorrectPostId_ShouldReturnFalse()
        {
            var dbUser = new User();
            var service = this.GetService(dbUser);

            var incorrectPostId = Guid.NewGuid().ToString();
            var result = await this.CallRemoveDownVoteToPostAsync(incorrectPostId, service);

            Assert.False(result);
        }

        [Fact]
        public async Task WithoutDownVote_ShouldReturnFalse()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var service = this.GetService(dbUser, dbPost);

            var result = await this.CallRemoveDownVoteToPostAsync(dbPost.Id, service);

            Assert.False(result);
        }

        [Fact]
        public async Task WithUpVote_ShouldReturnFalse()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateUpVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            var result = await this.CallRemoveDownVoteToPostAsync(dbPost.Id, service);

            Assert.False(result);
        }

        [Fact]
        public async Task WithDownVote_ShouldReturnTrue()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            var result = await this.CallRemoveDownVoteToPostAsync(dbPost.Id, service);

            Assert.True(result);
        }

        [Fact]
        public async Task WithDownVote_ShouldDecreaseWithOnePostDownVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                DownVotesCount = 1
            };
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            var result = await this.CallRemoveDownVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(0, dbPost.DownVotesCount);
        }

        [Fact]
        public async Task WithDownVote_ShouldRemoveVotePostFromDatabase()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            this.AddEnitytiesToUnitOfWork(unitOfWork, dbUser, dbPost, dbVotePost);
            var service = this.GetService(unitOfWork, dbUser.Id);

            var result = await this.CallRemoveDownVoteToPostAsync(dbPost.Id, service);

            Assert.Empty(unitOfWork.VotePostRepository.GetAll());
        }

        private async Task<bool> CallRemoveDownVoteToPostAsync(string postId, IUserVotePostService service)
        {
            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;
            var result = await service.RemoveDownVoteToPostAsync(postId, claimsPrincipal);

            return result;
        }
    }
}
