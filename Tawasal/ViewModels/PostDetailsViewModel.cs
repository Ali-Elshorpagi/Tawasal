using Tawasal.Models;

namespace Tawasal.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public bool IsLikedByCurrentUser { get; set; }
    }
}
