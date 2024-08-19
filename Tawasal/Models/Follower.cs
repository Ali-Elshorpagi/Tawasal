namespace Tawasal.Models
{
    public class Follower
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid FollowerProfileId { get; set; }
        public Profile FollowerProfile { get; set; } = null!;
        public Guid FollowedProfileId { get; set; }
        public Profile FollowedProfile { get; set; } = null!;
    }
}
