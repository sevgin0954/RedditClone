using RedditClone.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace RedditClone.Models.WebModels.CommentModels.BindingModels
{
    public class CommentBindingModel
    {
        [Required]
        public string PostId { get; set; }

        [Required]
        public string SourceId { get; set; }

        [Required]
        [MinLength(ModelsConstants.DescriptionCommentMinLength)]
        [MaxLength(ModelsConstants.DescriptionCommentMaxLength)]
        public string Description { get; set; }
    }
}
