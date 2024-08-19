using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tawasal.Services.IServices;
using Tawasal.ViewModels;
using System.Security.Claims;

namespace Tawasal.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id) // ApplicationUser Id
        {
            if (id is null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId != id || userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(id);

            if (profile is null)
                return NotFound();

            ViewBag.CurrentUserId = userId;
            ViewBag.ProfileName = profile.FirstName;
            ViewBag.Friends = await _profileService.GetFriendsAsync(profile.Id);
            ViewBag.Followings = await _profileService.GetFollowingsAsync(profile.Id);

            return View(profile);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
            if (id is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(id);

            if (profile is null)
                return NotFound();

            var viewModel = new ProfileUpdateViewModel
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                // ProfilePicturePath = profile.ProfilePicturePath,
                Bio = profile.Bio,
                Address = profile.Address,
                BirthofDate = profile.BirthofDate,
                ApplicationUserId = profile.ApplicationUserId
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, ProfileUpdateViewModel viewModel)
        {
            if (id != viewModel.ApplicationUserId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var profile = await _profileService.GetProfileByUserIdAsync(id);

                    if (profile is null)
                        return NotFound();

                    if (viewModel.ProfilePicture is not null && viewModel.ProfilePicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ProfilePicture.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfilePictures", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.ProfilePicture.CopyToAsync(stream);
                        }

                        profile.ProfilePicturePath = $"{fileName}";
                    }

                    profile.FirstName = viewModel.FirstName;
                    profile.LastName = viewModel.LastName;
                    profile.Bio = viewModel.Bio;
                    profile.Address = viewModel.Address;
                    profile.BirthofDate = viewModel.BirthofDate;
                    profile.ApplicationUserId = viewModel.ApplicationUserId;

                    await _profileService.UpdateProfileAsync(profile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _profileService.GetProfileByUserIdAsync(viewModel.ApplicationUserId) is null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", new { id = viewModel.ApplicationUserId });
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(Guid followedProfileId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _profileService.FollowProfileAsync(profile.Id, followedProfileId);

            var followedUserId = await _profileService.GetUserIdByProfileIdAsync(followedProfileId);

            return RedirectToAction("ViewProfile", new { id = followedUserId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfollow(Guid followedProfileId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _profileService.UnfollowProfileAsync(profile.Id, followedProfileId);

            var followedUserId = await _profileService.GetUserIdByProfileIdAsync(followedProfileId);

            return RedirectToAction("ViewProfile", new { id = followedUserId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendFriendRequest(Guid receiverId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var receiverProfile = await _profileService.GetProfileByIdAsync(receiverId);
            if (receiverProfile is null)
                return NotFound();

            await _profileService.SendFriendRequestAsync(profile.Id, receiverId);

            return RedirectToAction("ViewProfile", new { id = receiverProfile.ApplicationUserId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptFriendRequest(Guid requestId, Guid profileId)
        {
            if (requestId == Guid.Empty || profileId == Guid.Empty)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            await _profileService.AcceptFriendRequestAsync(requestId);

            var profile = await _profileService.GetProfileByIdAsync(profileId);

            return RedirectToAction("ViewProfile", new { id = profile.ApplicationUserId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineFriendRequest(Guid requestId, Guid profileId)
        {
            if (requestId == Guid.Empty || profileId == Guid.Empty)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            await _profileService.DeclineFriendRequestAsync(requestId);

            var profile = await _profileService.GetProfileByIdAsync(profileId);

            return RedirectToAction("ViewProfile", new { id = profile.ApplicationUserId });
        }
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var notifications = await _profileService.GetNotificationsByProfileIdAsync(profile.Id);

            return View(notifications);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkNotificationsAsSeen()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _profileService.MarkNotificationsAsSeenAsync(profile.Id);
            return RedirectToAction("Notifications");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _profileService.DeleteAllNotificationsAsync(profile.Id);

            return RedirectToAction("Notifications");
        }
        [HttpGet]
        public async Task<IActionResult> Followers()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var followers = await _profileService.GetFollowersAsync(profile.Id);

            return View(followers);
        }
        [HttpGet]
        public async Task<IActionResult> Followings()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var followings = await _profileService.GetFollowingsAsync(profile.Id);

            return View(followings);
        }
        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var friends = await _profileService.GetFriendsAsync(profile.Id);

            return View(friends);
        }
        [HttpGet]
        public async Task<IActionResult> Posts()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var posts = await _profileService.GetPostsByProfileIdAsync(profile.Id);

            return View(posts);
        }
        [HttpGet]
        public async Task<IActionResult> FriendRequests()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var friendRequests = await _profileService.GetFriendRequestsAsync(profile.Id);

            return View(friendRequests);
        }
        [HttpGet]
        public async Task<IActionResult> ViewProfile(string? id) // ApplicationUser Id
        {
            if (id is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(id);
            if (profile is null)
                return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var currentUserProfile = await _profileService.GetProfileByUserIdAsync(userId);

            var isFriend = await _profileService.AreProfilesFriendsAsync(currentUserProfile.Id, profile.Id);
            var isFollowing = await _profileService.IsFollowingAsync(currentUserProfile.Id, profile.Id);
            var hasSentFriendRequest = await _profileService.HasSentFriendrequest(currentUserProfile.Id, profile.Id);
            var hasReceivedFriendRequest = await _profileService.HasReceivedFriendRequestAsync(currentUserProfile.Id, profile.Id);

            var viewModel = new ViewProfileViewModel
            {
                Profile = profile,
                IsFriend = isFriend,
                IsFollowing = isFollowing,
                HasSentFriendRequest = hasSentFriendRequest,
                HasReceivedFriendRequest = hasReceivedFriendRequest.HasReceivedRequest,
                FriendRequestId = hasReceivedFriendRequest.RequestId
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelFriendRequest(Guid receiverId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var senderProfile = await _profileService.GetProfileByUserIdAsync(userId);
            if (senderProfile is null)
                return NotFound();

            await _profileService.CancelFriendRequestAsync(senderProfile.Id, receiverId);

            var receiverUserId = await _profileService.GetUserIdByProfileIdAsync(receiverId);

            return RedirectToAction("ViewProfile", new { id = receiverUserId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFriend(Guid friendProfileId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _profileService.RemoveFriendAsync(profile.Id, friendProfileId);

            var friendUserId = await _profileService.GetUserIdByProfileIdAsync(friendProfileId);

            return RedirectToAction("ViewProfile", new { id = friendUserId });
        }
    }
}
