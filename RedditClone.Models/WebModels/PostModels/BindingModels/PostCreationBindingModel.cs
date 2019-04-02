using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedditClone.Models.WebModels.PostModels.BindingModels
{
    public class PostCreationBindingModel
    {
        [Required]
        public string AuthorId { get; set; }

        [Required]
        [MinLength(ModelsConstants.TitlePostMinLength)]
        [MaxLength(ModelsConstants.TitlePostMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ModelsConstants.DescriptionPostMinLength)]
        [MaxLength(ModelsConstants.DescriptionPostMaxLength)]
        public string Description { get; set; }

        [Required]
        public string SelectedSubredditId { get; set; }
        public List<SelectListItem> Subreddits { get; set; } = new List<SelectListItem>();
    }
}
