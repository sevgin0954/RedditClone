using Moq;
using RedditClone.Models;
using RedditClone.Services.UserServices.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Services.Tests.UserServicesTests.UserVotePostServiceTests
{
    public class AddUpVoteToPostAsyncTests : BaseUserVoteServiceTest
    {
        [Fact]
        public async Task WithIncorrectPostId_ShouldReturnFalse()
        {
            var dbUser = new User();
            var service = this.GetService(dbUser);

            var incorrectPostId = Guid.NewGuid().ToString();
            var result = await this.CallAddUpVoteToPostAsync(incorrectPostId, service);

            Assert.False(result);
        }

        [Fact]
        public async Task WithCorrectPostId_ShouldReturnTrue()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var service = this.GetService(dbUser, dbPost);

            var result = await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.True(result);
        }

        [Fact]
        public async Task WithCorrectPostId_ShouldIncreaseWithOneUpVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var service = this.GetService(dbUser, dbPost);

            await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(1, dbPost.UpVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionDownVote_ShouldDecreaseWithOneDownVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                DownVotesCount = 1
            };
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(0, dbPost.DownVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithoutVote_ShouldNotDecreaseWithOneDownVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                DownVotesCount = 1
            };
            var service = this.GetService(dbUser, dbPost);

            await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(1, dbPost.DownVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithoutPrevisionVote_ShouldCreateNewVotePostWithPost()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbPost = new Post();
            this.AddEnitytiesToUnitOfWork(unitOfWork, dbUser, dbPost);

            var service = this.GetService(unitOfWork, dbUser.Id);

            await this.CallAddUpVoteToPostAsync(dbPost.Id, service);
            var dbVotePost = unitOfWork.VotePostRepository.GetAll().First();

            Assert.Equal(dbPost.Id, dbVotePost.PostId);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionDownVote_ShouldChangeVotePostValueToPositive()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(1, dbVotePost.Value);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionUpVote_ShouldReturnFalse()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateUpVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            var result = await this.CallAddUpVoteToPostAsync(dbPost.Id, service);

            Assert.False(result);
        }

        private async Task<bool> CallAddUpVoteToPostAsync(string postId, IUserVotePostService service)
        {
            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;
            var result = await service.AddUpVoteToPostAsync(postId, claimsPrincipal);

            return result;
        }
    }
}
