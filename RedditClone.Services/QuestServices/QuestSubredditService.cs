using AutoMapper;
using RedditClone.Common.Enums;
using RedditClone.Common.Enums.SortTypes;
using RedditClone.Common.Enums.TimeFrameTypes;
using RedditClone.Data.Factories.SortFactories;
using RedditClone.Data.Factories.TimeFactories;
using RedditClone.Data.Interfaces;
using RedditClone.Models.WebModels.SubredditModels.ViewModels;
using RedditClone.Services.QuestServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditClone.Services.QuestServices
{
    public class QuestSubredditService : IQuestSubredditService
    {
        private readonly IRedditCloneUnitOfWork redditCloneUnitOfWork;
        private readonly IMapper mapper;

        public QuestSubredditService(IRedditCloneUnitOfWork redditCloneUnitOfWork, IMapper mapper)
        {
            this.redditCloneUnitOfWork = redditCloneUnitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SubredditConciseViewModel>> GetOrderedSubredditsByKeyWords(
            string[] keyWords,
            SubredditSortType sortType)
        {
            var sortStrategy = SortSubredditStrategyFactory
                .GetSubredditSortStrategy(this.redditCloneUnitOfWork, sortType);

            var filteredSubreddits = await this.redditCloneUnitOfWork.Subreddits
                .GetByKeyWordsSortedByAsync(keyWords, sortStrategy);

            var models = this.mapper.Map<IEnumerable<SubredditConciseViewModel>>(filteredSubreddits);

            return models;
        }
    }
}
