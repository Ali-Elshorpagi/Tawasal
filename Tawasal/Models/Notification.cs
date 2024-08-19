using Tawasal.Helpers;

namespace Tawasal.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsSeen { get; set; } = false;
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
         public NotificationType Type { get; set; } 
        public string? ReferenceId { get; set; } 
    }
}
