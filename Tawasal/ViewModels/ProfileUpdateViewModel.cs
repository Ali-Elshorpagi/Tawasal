using System.ComponentModel.DataAnnotations;

namespace Tawasal.ViewModels
{
    public class ProfileUpdateViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        // public string ProfilePicturePath { get; set; } = "default.jpg";
        public IFormFile ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthofDate { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
