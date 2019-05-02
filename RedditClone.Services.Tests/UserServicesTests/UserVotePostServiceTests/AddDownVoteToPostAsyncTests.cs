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
    public class AddDownVoteToPostAsyncTests : BaseUserVoteServiceTest
    {
        [Fact]
        public async Task WithIncorrectPostId_ShouldReturnFalse()
        {
            var dbUser = new User();
            var service = this.GetService(dbUser);
            
            var incorrectPostId = Guid.NewGuid().ToString();
            var result = await this.CallAddDownVoteToPostAsync(incorrectPostId, service);

            Assert.False(result);
        }

        [Fact]
        public async Task WithCorrectPostId_ShouldReturnTrue()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var service = this.GetService(dbUser, dbPost);

            var result = await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.True(result);
        }

        [Fact]
        public async Task WithCorrectPostId_ShouldIncreaseWithOneDownVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var service = this.GetService(dbUser, dbPost);
            
            await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(1, dbPost.DownVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionPositiveVote_ShouldDecreaseWithOneUpVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                UpVotesCount = 1
            };
            var dbVotePost = this.CreateUpVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(0, dbPost.UpVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithoutVote_ShouldNotDecreaseWithOneUpVotesCount()
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                UpVotesCount = 1
            };
            var service = this.GetService(dbUser, dbPost);

            await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(1, dbPost.UpVotesCount);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithoutPrevisionVote_ShouldCreateNewVotePostWithPost()
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            var dbUser = new User();
            var dbPost = new Post();
            this.AddEnitytiesToUnitOfWork(unitOfWork, dbUser, dbPost);

            var service = this.GetService(unitOfWork, dbUser.Id);

            await this.CallAddDownVoteToPostAsync(dbPost.Id, service);
            var dbVotePost = unitOfWork.VotePostRepository.GetAll().First();

            Assert.Equal(dbPost.Id, dbVotePost.PostId);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionUpVote_ShouldChangeVotePostValueToNegative()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateUpVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.Equal(-1, dbVotePost.Value);
        }

        [Fact]
        public async Task WithCorrectPostIdAndUserWithPrevisionDownVote_ShouldReturnFalse()
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbVotePost = this.CreateDownVotePost(dbUser, dbPost);
            var service = this.GetService(dbUser, dbPost, dbVotePost);

            var result = await this.CallAddDownVoteToPostAsync(dbPost.Id, service);

            Assert.False(result);
        }

        private async Task<bool> CallAddDownVoteToPostAsync(string postId, IUserVotePostService service)
        {
            var claimsPrincipal = new Mock<ClaimsPrincipal>().Object;
            var result = await service.AddDownVoteToPostAsync(postId, claimsPrincipal);

            return result;
        }
    }
}
