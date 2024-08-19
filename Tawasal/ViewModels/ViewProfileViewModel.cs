using Tawasal.Models;

namespace Tawasal.ViewModels
{
    public class ViewProfileViewModel
    {
        public Profile Profile { get; set; }
        public bool IsFriend { get; set; }
        public bool IsFollowing { get; set; }
        public bool HasSentFriendRequest { get; set; }
        public bool HasReceivedFriendRequest { get; set; }
        public Guid? FriendRequestId { get; set; }
    }
}
