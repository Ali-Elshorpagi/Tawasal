using System.ComponentModel.DataAnnotations;

namespace Tawasal.ViewModels
{
    public class EditCommentViewModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        [Required(ErrorMessage = "You can\'t add an empty post ")]
        public string Content { get; set; } = null!;
    }
}
