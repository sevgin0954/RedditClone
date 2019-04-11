using AutoMapper;
using RedditClone.Common.Enums;
using RedditClone.Data.Interfaces;
using RedditClone.Models.WebModels.CommentModels.ViewModels;
using RedditClone.Models.WebModels.PostModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestPostService : IQuestPostService
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public QuestPostService(IRedditCloneUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PostViewModel> GetPostModelAsync(string postId, SortType sortType)
        {
            var dbPost = await unitOfWork.Posts.GetByIdWithIncludedAllProperties(postId);
            if (dbPost == null)
            {
                return null;
            }

            var model = this.mapper.Map<PostViewModel>(dbPost);
            model.SelectedSortType = sortType;
            model.CommentsCount = this.CountComments(model.Comments.ToList());

            return model;
        }

        private int CountComments(ICollection<CommentViewModel> comments)
        {
            if (comments.Count == 0)
            {
                return 0;
            }

            int totalCommentsCount = comments.Count;

            foreach (var comment in comments)
            {
                int RepliesCommentsCount = this.CountComments(comment.Replies);
                totalCommentsCount += RepliesCommentsCount;
            }

            return totalCommentsCount;
        }
    }
}
