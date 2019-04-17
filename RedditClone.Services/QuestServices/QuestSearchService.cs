using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Models.WebModels.SearchModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestSearchService : IQuestSearchService
    {
        private readonly IQuestPostService questPostService;
        private readonly IQuestSubredditService questSubredditService;

        public QuestSearchService(
            IQuestPostService questPostService, 
            IQuestSubredditService questSubredditService)
        {
            this.questPostService = questPostService;
            this.questSubredditService = questSubredditService;
        }

        public async Task<SearchResultViewModel> GetSearchResultAsync(
            string[] keyWords, 
            SubredditSortType subredditSortType,
            PostSortType postSortType,
            TimeFrameType postTimeFrameType)
        {
            var subredditModels = await this.questSubredditService
                .GetOrderedSubredditsByKeyWords(keyWords, subredditSortType);
            var postModels = await this.questPostService
                .GetOrderedPostsByKeyWordsAsync(keyWords, postSortType, postTimeFrameType);

            var searchModel = new SearchResultViewModel
            {
                Subreddits = subredditModels,
                Posts = postModels,
                KeyWords = string.Join(" ", keyWords)
            };

            return searchModel;
        }
    }
}
