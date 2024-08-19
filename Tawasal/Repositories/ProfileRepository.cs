using Microsoft.EntityFrameworkCore;
using Tawasal.Contexts;
using Tawasal.Helpers;
using Tawasal.Models;
using Tawasal.Repositories.IRepositories;

namespace Tawasal.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationContext _context;
        public ProfileRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            var profile = await _context.Profiles
                            .AsSplitQuery()
                            .Include(p => p.ApplicationUser)
                            .Include(p => p.Posts)
                            .Include(p => p.Friends)
                            .Include(p => p.FriendRequests)
                            .Include(p => p.Notifications)
                            .Include(p => p.Followers)
                            .FirstOrDefaultAsync(u => u.Id == id);
            return profile!;
        }
        public async Task UpdateProfileAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }
        public async Task<Profile> GetProfileByUserIdAsync(string userId)
        {
            var profile = await _context.Profiles
                            .AsSplitQuery()
                            .Include(p => p.ApplicationUser)
                            .Include(p => p.Posts)
                            .Include(p => p.FriendRequests)
                            .Include(p => p.Notifications)
                            .Include(p => p.Followers)
                            .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
            return profile!;
        }
        public async Task FollowProfileAsync(Guid followerProfileId, Guid followedProfileId)
        {
            var followerProfileExists = await _context.Profiles.AnyAsync(p => p.Id == followerProfileId);
            var followedProfileExists = await _context.Profiles.AnyAsync(p => p.Id == followedProfileId);

            if (!followerProfileExists)
                throw new InvalidOperationException($"{followerProfileId} profile do not exist.");

            if (!followedProfileExists)
                throw new InvalidOperationException($"{followedProfileId} profile do not exist.");

            var follow = new Follower
            {
                FollowerProfileId = followerProfileId,
                FollowedProfileId = followedProfileId
            };
            await _context.Followers.AddAsync(follow);

            var following = new Following
            {
                FollowingProfileId = followerProfileId,
                FollowedProfileId = followedProfileId
            };
            await _context.Followings.AddAsync(following);

            var notification = new Notification
            {
                ProfileId = followedProfileId,
                Content = "You have a new follower",
                Type = NotificationType.Follower
            };
            await AddNotificationAsync(notification);

            await _context.SaveChangesAsync();
        }
        public async Task UnfollowProfileAsync(Guid followerProfileId, Guid followedProfileId)
        {
            var follow = await _context.Followers
                .FirstOrDefaultAsync(f => f.FollowerProfileId == followerProfileId && f.FollowedProfileId == followedProfileId);

            var following = await _context.Followings
                .FirstOrDefaultAsync(f => f.FollowingProfileId == followerProfileId && f.FollowedProfileId == followedProfileId);

            if (follow is not null && following is not null)
            {
                _context.Followers.Remove(follow);
                _context.Followings.Remove(following);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SendFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            var friendRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendRequestStatus.Pending
            };

            await _context.FriendRequests.AddAsync(friendRequest);

            var notification = new Notification
            {
                ProfileId = receiverId,
                Content = "You have a new friend request",
                Type = NotificationType.FriendRequest
            };

            await AddNotificationAsync(notification);

            await _context.SaveChangesAsync();
        }
        public async Task AcceptFriendRequestAsync(Guid requestId)
        {
            var request = await _context.FriendRequests.FirstOrDefaultAsync(re => re.Id == requestId);
            Console.WriteLine($"\n\n\n\n\n\nRequestedIDfromDatabase: {request?.Id}\n\n\n\n\n\n");
            if (request is not null && request.Status == FriendRequestStatus.Pending)
            {
                // request.Status = FriendRequestStatus.Accepted;
                // _context.FriendRequests.Update(request);

                var friendship = new Friend
                {
                    ProfileIdOne = request.SenderId,
                    ProfileIdTwo = request.ReceiverId
                };
                await _context.Friends.AddAsync(friendship);

                var notification = new Notification
                {
                    ProfileId = request.SenderId,
                    Content = "Your friend request has been accepted",
                    Type = NotificationType.Friend
                };
                await AddNotificationAsync(notification);
                _context.FriendRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeclineFriendRequestAsync(Guid requestId)
        {
            var request = await _context.FriendRequests.FindAsync(requestId);
            if (request is not null && request.Status == FriendRequestStatus.Pending)
            {
                _context.FriendRequests.Remove(request);

                var notification = new Notification
                {
                    ProfileId = request.SenderId,
                    Content = "Your friend request has been declined",
                    Type = NotificationType.General
                };
                await AddNotificationAsync(notification);

                await _context.SaveChangesAsync();
            }
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<Notification>> GetNotificationsByProfileIdAsync(Guid profileId)
        {
            return await _context.Notifications
                .AsSplitQuery()
                .Include(n => n.Profile)
                .Where(n => n.ProfileId == profileId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
        public async Task DeleteAllNotificationsAsync(Guid profileId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.ProfileId == profileId)
                .ToListAsync();

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }
        public async Task MarkNotificationsAsSeenAsync(Guid profileId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.ProfileId == profileId && !n.IsSeen)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsSeen = true;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<Profile>> GetFollowersAsync(Guid profileId)
        {
            return await _context.Followers
                .AsSingleQuery()
                .Where(f => f.FollowedProfileId == profileId)
                .Select(f => f.FollowerProfile)
                .ToListAsync();
        }
        public async Task<ICollection<Profile>> GetFriendsAsync(Guid profileId)
        {
            return await _context.Friends
                        .Where(f => f.ProfileIdOne == profileId || f.ProfileIdTwo == profileId)
                        .Select(f => f.ProfileIdOne == profileId ? f.ProfileTwo : f.ProfileOne)
                        .ToListAsync();
        }
        public async Task<ICollection<Profile>> GetFollowingsAsync(Guid profileId)
        {
            return await _context.Followings
                        .Where(f => f.FollowingProfileId == profileId)
                        .Select(f => f.FollowedProfile)
                        .ToListAsync();
        }
        public async Task<ICollection<FriendRequest>> GetFriendRequestsAsync(Guid profileId)
        {
            return await _context.FriendRequests
                .AsSplitQuery()
                .Where(fr => fr.ReceiverId == profileId && fr.Status == FriendRequestStatus.Pending)
                .Include(fr => fr.Sender)
                .ToListAsync();
        }
        public async Task<bool> AreProfilesFriendsAsync(Guid profileId1, Guid profileId2)
        {
            return await _context.Friends.AnyAsync(f =>
                (f.ProfileIdOne == profileId1 && f.ProfileIdTwo == profileId2) ||
                (f.ProfileIdOne == profileId2 && f.ProfileIdTwo == profileId1));
        }
        public async Task<bool> IsFollowingAsync(Guid followingId, Guid followedId)
        {
            var followings = await _context.Followings
                    .AnyAsync(f => f.FollowingProfileId == followingId && f.FollowedProfileId == followedId);
            var followers = await _context.Followers
                    .AnyAsync(f => f.FollowerProfileId == followingId && f.FollowedProfileId == followedId);
            return followers || followings;
        }
        public async Task<string> GetUserIdByProfileIdAsync(Guid profileId)
        {
            var userId = await _context.Profiles
                               .Where(p => p.Id == profileId)
                               .Select(p => p.ApplicationUserId)
                               .FirstOrDefaultAsync();
            return userId!;
        }
        public async Task<ICollection<Post>> GetPostsByProfileIdAsync(Guid profileId)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Where(p => p.ProfileId == profileId)
                .ToListAsync();
        }
        public async Task CancelFriendRequestAsync(Guid currentProfileId, Guid receiverId)
        {
            var friendRequest = await _context.FriendRequests
                                .FirstOrDefaultAsync(fr => fr.SenderId == currentProfileId && fr.ReceiverId == receiverId);
            if (friendRequest is not null)
            {
                _context.FriendRequests.Remove(friendRequest);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> HasSentFriendrequest(Guid currentProfileId, Guid receiverId)
        {
            return await _context.FriendRequests
                .AnyAsync(fr => fr.SenderId == currentProfileId && fr.ReceiverId == receiverId && fr.Status == FriendRequestStatus.Pending);
        }
        public async Task RemoveFriendAsync(Guid profileId, Guid friendProfileId)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => (f.ProfileIdOne == profileId && f.ProfileIdTwo == friendProfileId) ||
                                          (f.ProfileIdOne == friendProfileId && f.ProfileIdTwo == profileId));

            if (friendship is not null)
            {
                _context.Friends.Remove(friendship);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<(bool HasReceivedRequest, Guid? RequestId)> HasReceivedFriendRequestAsync(Guid currentUserId, Guid profileId)
        {
            var friendRequest = await _context.FriendRequests
                                   .FirstOrDefaultAsync(fr => fr.SenderId == profileId && fr.ReceiverId == currentUserId &&
                                                              fr.Status == FriendRequestStatus.Pending);
            return (friendRequest is not null, friendRequest?.Id);
        }
        public async Task<string?> GetPostOwnerIdAsync(Guid postId)
        {
            var userId = await _context.Posts
                                .Where(p => p.Id == postId)
                                .Select(p => p.Profile.ApplicationUserId)
                                .FirstOrDefaultAsync();
            return userId;
        }
        public async Task AddCommentNotificationAsync(Guid postId, string commenterProfileName)
        {
            var postOwnerId = await GetPostOwnerIdAsync(postId);
            if (postOwnerId is not null)
            {
                var profile = await GetProfileByUserIdAsync(postOwnerId);
                var notification = new Notification
                {
                    ProfileId = profile.Id,
                    Content = $"{commenterProfileName} commented on your post",
                    Type = NotificationType.Comment,
                    ReferenceId = postId.ToString()
                };

                await AddNotificationAsync(notification);
            }
        }
        public async Task AddLikeNotificationAsync(Guid postId, string likerProfileName)
        {
            var postOwnerId = await GetPostOwnerIdAsync(postId);
            if (postOwnerId is not null)
            {
                var profile = await GetProfileByUserIdAsync(postOwnerId);
                var notification = new Notification
                {
                    ProfileId = profile.Id,
                    Content = $"{likerProfileName} liked your post",
                    Type = NotificationType.Like,
                    ReferenceId = postId.ToString()
                };

                await AddNotificationAsync(notification);
            }
        }
    }
}
