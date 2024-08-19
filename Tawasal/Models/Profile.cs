using System.ComponentModel.DataAnnotations;

namespace Tawasal.Models
{
    public class Profile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(20), MinLength(3)]
        public string FirstName { get; set; }
        [MaxLength(20), MinLength(3)]
        public string? LastName { get; set; }
        public string ProfilePicturePath { get; set; } = "default.jpg";
        [MaxLength(120), MinLength(3)]
        public string? Bio { get; set; }
        [MaxLength(50), MinLength(3)]
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthofDate { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Friend> Friends { get; set; } = new List<Friend>();
        public virtual ICollection<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Follower> Followers { get; set; } = new List<Follower>();
        public virtual ICollection<Following> Followings { get; set; } = new List<Following>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
