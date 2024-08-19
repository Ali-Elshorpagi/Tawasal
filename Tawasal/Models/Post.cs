using System.ComponentModel.DataAnnotations;

namespace Tawasal.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "You can\'t add an empty post ")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
