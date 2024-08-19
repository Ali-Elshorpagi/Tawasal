using System.ComponentModel.DataAnnotations;

namespace Tawasal.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "You can\'t add an empty comment"), MaxLength(300)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
    }
}
