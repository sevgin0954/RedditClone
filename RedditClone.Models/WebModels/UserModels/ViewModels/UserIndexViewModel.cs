using System;

namespace RedditClone.Models.WebModels.UserModels.ViewModels
{
    public class UserIndexViewModel
    {
        public string PostId { get; set; }

        public string ActionInvokerId { get; set; }
        public string ActionInvokerUsername { get; set; }

        public string ActionName { get; set; }

        public string PostTitle { get; set; }

        public string SubredditId { get; set; }
        public string SubredditName { get; set; }

        public string PostCreatorId { get; set; }
        public string PostCreatorUsername { get; set; }

        public string DescriptionConcise { get; set; }

        public int VotesCount { get; set; }

        public DateTime PostDate { get; set; }
    }
}
