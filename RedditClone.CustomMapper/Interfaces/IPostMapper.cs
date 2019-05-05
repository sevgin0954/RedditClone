using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Data.SortStrategies.PostStrategies.Interfaces;
using RedditClone.Models;
using RedditClone.Models.WebModels.PostModels.BindingModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using System.Collections.Generic;

namespace RedditClone.CustomMapper.Interfaces
{
    public interface IPostMapper
    {
        PostsViewModel MapPostsViewModelForSignInUser(
            IEnumerable<Post> posts,
            string userId,
            PostSortType selectedSortType,
            ISortPostsStrategy sortStrategy,
            TimeFrameType selectedTimeFrameType);

        PostsViewModel MapPostsViewModelForQuest(
            IEnumerable<Post> posts,
            PostSortType selectedSortType,
            ISortPostsStrategy sortStrategy,
            TimeFrameType selectedTimeFrameType);

        PostCreationBindingModel MapPostCreationBindingModel(
            IEnumerable<Subreddit> createdSubreddits,
            IEnumerable<Subreddit> subscribedSubreddits,
            string selectedSubredditId);

        PostViewModel MapPostViewModelForQuest(Post post, CommentSortType sortType, IEnumerable<Comment> comments);

        Post MapPost(PostCreationBindingModel model, string authorId);

        PostViewModel MapPostViewModelForSignInUser(
            string userId,
            Post post,
            CommentSortType sortType,
            IEnumerable<Comment> comments);
    }
}
