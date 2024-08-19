using Tawasal.Models;

namespace Tawasal.Repositories.IRepositories
{
    public interface IProfileRepository
    {
        public Task<Profile> GetProfileByIdAsync(Guid id);
        public Task UpdateProfileAsync(Profile profile);
        public Task<Profile> GetProfileByUserIdAsync(string userId);
        public Task FollowProfileAsync(Guid followerProfileId, Guid followedProfileId);
        public Task UnfollowProfileAsync(Guid followerProfileId, Guid followedProfileId);
        public Task SendFriendRequestAsync(Guid senderId, Guid receiverId);
        public Task AcceptFriendRequestAsync(Guid requestId);
        public Task DeclineFriendRequestAsync(Guid requestId);
        public Task AddNotificationAsync(Notification notification);
        public Task<ICollection<Notification>> GetNotificationsByProfileIdAsync(Guid profileId);
        public Task DeleteAllNotificationsAsync(Guid profileId);
        public Task MarkNotificationsAsSeenAsync(Guid profileId);
        public Task<ICollection<Profile>> GetFollowersAsync(Guid profileId);
        public Task<ICollection<Profile>> GetFriendsAsync(Guid profileId);
        public Task<ICollection<Profile>> GetFollowingsAsync(Guid profileId);
        public Task<ICollection<Post>> GetPostsByProfileIdAsync(Guid profileId);
        public Task<ICollection<FriendRequest>> GetFriendRequestsAsync(Guid profileId);
        public Task<bool> AreProfilesFriendsAsync(Guid profileId1, Guid profileId2);
        public Task<bool> IsFollowingAsync(Guid followingId, Guid followedId);
        public Task<string> GetUserIdByProfileIdAsync(Guid profileId);
        public Task CancelFriendRequestAsync(Guid currentProfileId, Guid receiverId);
        public Task<bool> HasSentFriendrequest(Guid currentProfileId, Guid receiverId);
        public Task<(bool HasReceivedRequest, Guid? RequestId)> HasReceivedFriendRequestAsync(Guid currentUserId, Guid profileId);
        public Task RemoveFriendAsync(Guid profileId, Guid friendProfileId);
        public Task<string?> GetPostOwnerIdAsync(Guid postId);
        public Task AddCommentNotificationAsync(Guid postId, string commenterProfileName);
        public Task AddLikeNotificationAsync(Guid postId, string likerProfileName);
    }
}
