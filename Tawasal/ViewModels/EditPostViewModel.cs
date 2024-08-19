using System.ComponentModel.DataAnnotations;

namespace Tawasal.ViewModels
{
    public class EditPostViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "You can\'t add an empty post ")]
        public string Content { get; set; } = null!;
    }
}
