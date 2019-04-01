using Microsoft.AspNetCore.Mvc.Rendering;
using RedditClone.Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedditClone.Models.WebModels.PostModels.BindingModels
{
    public class CreationPostBindingModel
    {
        [Required]
        public string Id { get; set; }

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
        public string SubredditId { get; set; }

        [Required]
        public string SelectedSubredditId { get; set; }
        public ICollection<SelectListItem> CreatedSubreddits { get; set; } = new LinkedList<SelectListItem>();
        public ICollection<SelectListItem> SubscribedSubreddits { get; set; } = new List<SelectListItem>();
    }
}
