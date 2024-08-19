namespace Tawasal.Models
{
    public class Following
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid FollowingProfileId { get; set; }
        public Profile FollowingProfile { get; set; } = null!;
        public Guid FollowedProfileId { get; set; }
        public Profile FollowedProfile { get; set; } = null!;
    }
}
