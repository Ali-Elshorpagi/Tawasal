using System.ComponentModel.DataAnnotations;

namespace Tawasal.ViewModels
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "You can\'t add an empty post")]
        public string Content { get; set; }
    }
}
