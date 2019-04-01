using Moq;
using RedditClone.Common.Constants;
using RedditClone.Models;
using RedditClone.Models.WebModels.UserModels.ViewModels;
using RedditClone.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace RedditClone.Tests.UserAccountServiceTests.UserPostServiceTests
{
    public class PrepareIndexModelTests : BaseUserAccountServiceTest
    {
        [Fact]
        public async Task WithoutActivity_ShouldReturnEmptyCollection()
        {
            var dbUser = new User();

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);

            Assert.Empty(models);
        }

        [Fact]
        public async Task WithUserWithCreatedPostAndComment_ShouldReturnCorrectModelsCount()
        {
            var dbPost = new Post();
            var dbComment = new Comment();

            var dbUser = new User();
            dbUser.Posts.Add(dbPost);
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var modelsCount = models.Count();

            Assert.Equal(2, modelsCount);
        }

        [Fact]
        public async Task WithUserWithCreatedPostsAndCommentsWithPostDate_ShouldReturnModelsInDescendingOrderByPostDate()
        {
            var dbUser = new User();
            var dbComment1 = new Comment();
            var dbComment2 = new Comment();
            var dbPost1 = new Post();
            var dbPost2 = new Post();

            var initialDatetime = DateTime.UtcNow;

            dbComment1.PostDate = initialDatetime.AddDays(2);
            dbPost1.PostDate = initialDatetime.AddDays(1).AddSeconds(1);
            dbComment2.PostDate = initialDatetime.AddDays(1);
            dbPost2.PostDate = initialDatetime;

            dbUser.Posts.Add(dbPost1);
            dbUser.Posts.Add(dbPost2);
            dbUser.Comments.Add(dbComment1);
            dbUser.Comments.Add(dbComment2);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var modelsAsArray = models.ToArray();

            Assert.True(DateTime.Compare(modelsAsArray[0].PostDate, dbComment1.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[1].PostDate, dbPost1.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[2].PostDate, dbComment2.PostDate) == 0);
            Assert.True(DateTime.Compare(modelsAsArray[3].PostDate, dbPost2.PostDate) == 0);
        }

        [Fact]
        public async Task WithUserWithCreatedPostWithId_ShouldReturnModelWithCorrectPostId()
        {
            var dbPost = new Post();
            var dbUser = new User();
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelId = models.First().PostId;
            var dbPostId = dbPost.Id;

            Assert.Equal(dbPostId, firstModelId);
        }

        [Fact]
        public async Task WithUserWithCreatedCommentAtPostWithId_ShouldReturnModelWithCorrectPostId()
        {
            var dbPost = new Post();
            var dbComment = new Comment();
            var dbUser = new User();
            dbPost.Comments.Add(dbComment);
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelId = models.First().PostId;
            var dbPostId = dbPost.Id;

            Assert.Equal(dbPostId, firstModelId);
        }

        [Theory]
        [InlineData("Username")]
        public async Task WithUserWithNameAndCreatedComment_ShouldReturnModelWithCorrectActionInvokerIdAndUsername(
            string username)
        {
            var dbComment = new Comment();
            var dbUser = new User()
            {
                UserName = username
            };
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelActionInvokerUserUsername = firstModel.ActionInvokerUsername;
            var firstModelActionInvokerId = firstModel.ActionInvokerId;
            var dbUserUsername = dbUser.UserName;
            var dbUserId = dbUser.Id;

            Assert.Equal(dbUserId, firstModelActionInvokerId);
            Assert.Equal(dbUserUsername, firstModelActionInvokerUserUsername);
        }

        [Theory]
        [InlineData("Username")]
        public async Task WithUserWithNameAndCreatedPost_ShouldReturnModelWithCorrectActionInvokerIdAndUsername(
            string username)
        {
            var dbPost = new Post();
            var dbUser = new User()
            {
                UserName = username
            };
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelActionInvokerUserUsername = firstModel.ActionInvokerUsername;
            var firstModelActionInvokerId = firstModel.ActionInvokerId;
            var dbUserUsername = dbUser.UserName;
            var dbUserId = dbUser.Id;

            Assert.Equal(dbUserId, firstModelActionInvokerId);
            Assert.Equal(dbUserUsername, firstModelActionInvokerUserUsername);
        }

        [Fact]
        public async Task WithUserWithCreatedPost_ShouldReturnModelWithCorrectActionName()
        {
            var dbPost = new Post();
            var dbUser = new User();
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var modelActionName = models.First().ActionName;
            var expectedActionName = ModelsConstants.ActionNamePost;

            Assert.Equal(expectedActionName, modelActionName);
        }

        [Fact]
        public async Task WithUserWithCreatedComment_ShouldReturnModelWithCorrectActionName()
        {
            var dbComment = new Comment();
            var dbUser = new User();
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelActionName = models.First().ActionName;
            var expectedActionName = ModelsConstants.ActionNameComment;

            Assert.Equal(expectedActionName, firstModelActionName);
        }

        [Theory]
        [InlineData("Post title")]
        public async Task WithUserWithCreatedPostWithTitle_ShouldReturnModelWithCorrectPostTitle(string postTitle)
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                Title = postTitle
            };
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelPostTitle = models.First().PostTitle;

            Assert.Equal(postTitle, firstModelPostTitle);
        }

        [Theory]
        [InlineData("Post title")]
        public async Task WithUserWithCreatedCommentAtPostWithTitle_ShouldReturnModelWithCorrectPostTitle(
            string postTitle)
        {
            var dbUser = new User();
            var dbComment = new Comment();
            var dbPost = new Post()
            {
                Title = postTitle
            };
            dbComment.Post = dbPost;
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelPostTitle = models.First().PostTitle;

            Assert.Equal(postTitle, firstModelPostTitle);
        }

        [Theory]
        [InlineData("Subreddit name")]
        public async Task WithUserWithCreatedCommentAtPostWithSubredditWithName_ShouldReturnModelWithCorrectSubredditIdAndName(
            string subredditName)
        {
            var dbUser = new User();
            var dbComment = new Comment();
            var dbPost = new Post();
            var dbSubreddit = new Subreddit()
            {
                Name = subredditName
            };
            dbPost.Subreddit = dbSubreddit;
            dbComment.Post = dbPost;
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelSubredditName = firstModel.SubredditName;
            var fistModelSubredditId = firstModel.SubredditId;
            var dbSubredditId = dbSubreddit.Id;

            Assert.Equal(dbSubredditId, fistModelSubredditId);
            Assert.Equal(subredditName, firstModelSubredditName);
        }

        [Theory]
        [InlineData("Subreddit name")]
        public async Task WithUserWithCreatedPostWithSubredditWithName_ShouldReturnModelWithCorrectSubredditIdAndName(
            string subredditName)
        {
            var dbUser = new User();
            var dbPost = new Post();
            var dbSubreddit = new Subreddit()
            {
                Name = subredditName
            };
            dbPost.Subreddit = dbSubreddit;
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelSubredditName = firstModel.SubredditName;
            var fistModelSubredditId = firstModel.SubredditId;
            var dbSubredditId = dbSubreddit.Id;

            Assert.Equal(dbSubredditId, fistModelSubredditId);
            Assert.Equal(subredditName, firstModelSubredditName);
        }

        [Theory]
        [InlineData("Username")]
        public async Task WithUserWithNameAndWithCreatedCommentAndPost_ShouldReturnModelWithCorrectPostCreatorUserIdAndUsername(
            string creatorUsername)
        {
            var dbUser = new User()
            {
                UserName = creatorUsername
            };
            var dbComment = new Comment();
            var dbPost = new Post();
            dbComment.Post = dbPost;
            dbUser.Posts.Add(dbPost);
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelPostCreatorId = firstModel.PostCreatorId;
            var firstModelPostCreatorUsername = firstModel.PostCreatorUsername;
            var dbUserId = dbUser.Id;

            Assert.Equal(dbUserId, firstModelPostCreatorId);
            Assert.Equal(creatorUsername, firstModelPostCreatorUsername);
        }

        [Theory]
        [InlineData("Username")]
        public async Task WithUserWithNameAndWithCreatedPost_ShouldReturnModelWithCorrectPostCreatorUserIdAndUsername(
            string creatorUsername)
        {
            var dbUser = new User()
            {
                UserName = creatorUsername
            };
            var dbPost = new Post();
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModel = models.First();
            var firstModelPostCreatorId = firstModel.PostCreatorId;
            var firstModelPostCreatorUsername = firstModel.PostCreatorUsername;
            var dbUserId = dbUser.Id;
            

            Assert.Equal(dbUserId, firstModelPostCreatorId);
            Assert.Equal(creatorUsername, firstModelPostCreatorUsername);
        }

        [Fact]
        public async Task WithUserWithCreatedCommentWithLongDescription_ShouldReturnModelWithCorrectDescriptionConcise()
        {
            var description = new string('a', ModelsConstants.DescriptionPreviewLength * 2);

            var dbUser = new User();
            var dbComment = new Comment()
            {
                Description = description
            };
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelDescriptionConcise = models.First().DescriptionConcise;
            var expectedDescription = description.Substring(0, ModelsConstants.DescriptionPreviewLength);

            Assert.Equal(expectedDescription, firstModelDescriptionConcise);
        }

        [Fact]
        public async Task WithUserWithCreatedPostWithLongDescription_ShouldReturnModelWithCorrectDescriptionConcise()
        {
            var description = new string('a', ModelsConstants.DescriptionPreviewLength * 2);

            var dbUser = new User();
            var dbPost = new Post()
            {
                Description = description
            };

            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelDescriptionConcise = models.First().DescriptionConcise;
            var expectedDescription = description.Substring(0, ModelsConstants.DescriptionPreviewLength);

            Assert.Equal(expectedDescription, firstModelDescriptionConcise);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(5)]
        public async Task WithUserWithCreatedCommentWithVotes_ShouldReturnModelWithCorrectVotesCount(int votesCount)
        {
            var dbUser = new User();
            var dbComment = new Comment()
            {
                VotesCount = votesCount
            };
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelVotesCount = models.First().VotesCount;

            Assert.Equal(votesCount, firstModelVotesCount);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(5)]
        public async Task WithUserWithCreatedPostWithVotes_ShouldReturnModelWithCorrectVotesCount(int votesCount)
        {
            var dbUser = new User();
            var dbPost = new Post()
            {
                VotesCount = votesCount
            };
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelVotesCount = models.First().VotesCount;

            Assert.Equal(votesCount, firstModelVotesCount);
        }

        [Fact]
        public async Task WithUserWithCreatedCommentAtDate_ShouldReturnModelWithCorrectPostDate()
        {
            var dateTime = DateTime.UtcNow;

            var dbUser = new User();
            var dbComment = new Comment()
            {
                PostDate = dateTime
            };
            dbUser.Comments.Add(dbComment);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelPostDate = models.First().PostDate;

            Assert.Equal(dateTime, firstModelPostDate);
        }

        [Fact]
        public async Task WithUserWithCreatedPostAtDate_ShouldReturnModelWithCorrectPostDate()
        {
            var dateTime = DateTime.UtcNow;

            var dbUser = new User();
            var dbPost = new Post()
            {
                PostDate = dateTime
            };
            dbUser.Posts.Add(dbPost);

            var models = await this.CallPrepareIndexModelAsyncWithUserAsync(dbUser);
            var firstModelPostDate = models.First().PostDate;

            Assert.Equal(dateTime, firstModelPostDate);
        }

        private async Task<IEnumerable<UserIndexViewModel>> CallPrepareIndexModelAsyncWithUserAsync(User user)
        {
            var unitOfWork = this.GetRedditCloneUnitOfWork();
            unitOfWork.Users.Add(user);
            unitOfWork.Complete();

            var mockedUserManager = this.GetMockedUserManager();
            CommonTestMethods.SetupMockedUserManagerGetUserAsync(mockedUserManager, user);

            var service = this.GetService(unitOfWork, mockedUserManager.Object);
            var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

            var models = await service.PrepareIndexModelAsync(mockedClaimsPrincipal.Object);
            return models;
        }
    }
}
