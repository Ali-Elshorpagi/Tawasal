using Tawasal.Models;
using Tawasal.Repositories.IRepositories;
using Tawasal.Services.IServices;

namespace Tawasal.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            return await _profileRepository.GetProfileByIdAsync(id);
        }
        public async Task UpdateProfileAsync(Profile profile)
        {
            await _profileRepository.UpdateProfileAsync(profile);
        }
        public async Task<Profile> GetProfileByUserIdAsync(string userId)
        {
            return await _profileRepository.GetProfileByUserIdAsync(userId);
        }
        public async Task FollowProfileAsync(Guid followerProfileId, Guid followedProfileId)
        {
            await _profileRepository.FollowProfileAsync(followerProfileId, followedProfileId);
        }
        public async Task UnfollowProfileAsync(Guid followerProfileId, Guid followedProfileId)
        {
            await _profileRepository.UnfollowProfileAsync(followerProfileId, followedProfileId);
        }
        public async Task SendFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            await _profileRepository.SendFriendRequestAsync(senderId, receiverId);
        }
        public async Task AcceptFriendRequestAsync(Guid requestId)
        {
            await _profileRepository.AcceptFriendRequestAsync(requestId);
        }
        public async Task DeclineFriendRequestAsync(Guid requestId)
        {
            await _profileRepository.DeclineFriendRequestAsync(requestId);
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            await _profileRepository.AddNotificationAsync(notification);
        }
        public async Task<ICollection<Notification>> GetNotificationsByProfileIdAsync(Guid profileId)
        {
            return await _profileRepository.GetNotificationsByProfileIdAsync(profileId);
        }
        public async Task DeleteAllNotificationsAsync(Guid profileId)
        {
            await _profileRepository.DeleteAllNotificationsAsync(profileId);
        }
        public async Task MarkNotificationsAsSeenAsync(Guid profileId)
        {
            await _profileRepository.MarkNotificationsAsSeenAsync(profileId);
        }
        public async Task<ICollection<Profile>> GetFollowersAsync(Guid profileId)
        {
            return await _profileRepository.GetFollowersAsync(profileId);
        }
        public async Task<ICollection<Profile>> GetFriendsAsync(Guid profileId)
        {
            return await _profileRepository.GetFriendsAsync(profileId);
        }
        public async Task<ICollection<FriendRequest>> GetFriendRequestsAsync(Guid profileId)
        {
            return await _profileRepository.GetFriendRequestsAsync(profileId);
        }
        public async Task<bool> AreProfilesFriendsAsync(Guid profileId1, Guid profileId2)
        {
            return await _profileRepository.AreProfilesFriendsAsync(profileId1, profileId2);
        }
        public async Task<bool> IsFollowingAsync(Guid followingId, Guid followedId)
        {
            return await _profileRepository.IsFollowingAsync(followingId, followedId);
        }
        public async Task<string> GetUserIdByProfileIdAsync(Guid profileId)
        {
            return await _profileRepository.GetUserIdByProfileIdAsync(profileId);
        }
        public async Task<ICollection<Post>> GetPostsByProfileIdAsync(Guid profileId)
        {
            return await _profileRepository.GetPostsByProfileIdAsync(profileId);
        }
        public async Task CancelFriendRequestAsync(Guid currentProfileId, Guid receiverId)
        {
            await _profileRepository.CancelFriendRequestAsync(currentProfileId, receiverId);
        }
        public async Task<bool> HasSentFriendrequest(Guid currentProfileId, Guid receiverId)
        {
            return await _profileRepository.HasSentFriendrequest(currentProfileId, receiverId);
        }
        public async Task RemoveFriendAsync(Guid profileId, Guid friendProfileId)
        {
            await _profileRepository.RemoveFriendAsync(profileId, friendProfileId);
        }
        public async Task<(bool HasReceivedRequest, Guid? RequestId)> HasReceivedFriendRequestAsync(Guid currentUserId, Guid profileId)
        {
            return await _profileRepository.HasReceivedFriendRequestAsync(currentUserId, profileId);
        }
        public async Task<string?> GetPostOwnerIdAsync(Guid postId)
        {
            return await _profileRepository.GetPostOwnerIdAsync(postId);
        }
        public async Task AddCommentNotificationAsync(Guid postId, string commenterProfileName)
        {
            await _profileRepository.AddCommentNotificationAsync(postId, commenterProfileName);
        }
        public async Task AddLikeNotificationAsync(Guid postId, string likerProfileName)
        {
            await _profileRepository.AddLikeNotificationAsync(postId, likerProfileName);
        }
        public async Task<ICollection<Profile>> GetFollowingsAsync(Guid profileId)
        {
            return await _profileRepository.GetFollowingsAsync(profileId);
        }
    }
}
