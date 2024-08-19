using System.Collections;
using Tawasal.Models;

namespace Tawasal.ViewModels
{
    public class TimeLineViewModel
    {
        public ICollection<Post> FriendsPosts { get; set; } = new List<Post>();
        public ICollection<Post> FollowingsPosts { get; set; } = new List<Post>();
        public ICollection<Post> MyPosts { get; set; } = new List<Post>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
