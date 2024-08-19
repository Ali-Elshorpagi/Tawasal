using Tawasal.Helpers;

namespace Tawasal.Models
{
    public class FriendRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Empty;
        public Guid SenderId { get; set; }
        public Profile Sender { get; set; } = null!;
        public Guid ReceiverId { get; set; }
        public Profile Receiver { get; set; } = null!;
    }
}
