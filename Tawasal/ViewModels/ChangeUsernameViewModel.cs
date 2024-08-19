using System.ComponentModel.DataAnnotations;

namespace Tawasal.ViewModels
{
    public class ChangeUsernameViewModel
    {
        [Required]
        public string CurrentUsername { get; set; }
        [Required]
        [Display(Name = "New Username")]
        public string NewUsername { get; set; }
    }
}
