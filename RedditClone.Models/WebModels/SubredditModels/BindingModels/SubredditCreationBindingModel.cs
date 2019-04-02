using RedditClone.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace RedditClone.Models.WebModels.SubredditModels.BindingModels
{
    public class SubredditCreationBindingModel
    {
        [Required]
        [MinLength(ModelsConstants.TitleSubredditMinLength)]
        [MaxLength(ModelsConstants.TitleSubredditMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(ModelsConstants.DescriptionSubredditMinLength)]
        [MaxLength(ModelsConstants.DescriptionSubredditMaxLength)]
        public string Description { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}
