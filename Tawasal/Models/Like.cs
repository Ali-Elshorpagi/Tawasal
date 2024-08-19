namespace Tawasal.Models
{
    public class Like
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
    }
}
