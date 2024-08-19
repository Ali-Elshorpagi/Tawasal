using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tawasal.Helpers;
using Tawasal.Models;
using Tawasal.Services.IServices;
using Tawasal.ViewModels;
using System.Security.Claims;

namespace Tawasal.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IFeedService _feedService;
        public FeedController(IProfileService profileService, IFeedService feedService)
        {
            _profileService = profileService;
            _feedService = feedService;
        }
        public async Task<IActionResult> TimeLine()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);

            if (profile is null)
                return NotFound();

            var friendsPostsTask = await _feedService.GetFriendsPostsByProfileIdAsync(profile.Id);
            var followingsPostsTask = await _feedService.GetFollowingPostsByProfileIdAsync(profile.Id);
            var myPostsTask = await _feedService.GetMyPostsByProfileIdAsync(profile.Id);

            // await Task.WhenAll(friendsPostsTask, followingsPostsTask, myPostsTask);

            var allPosts = new HashSet<Post>(
                friendsPostsTask.Concat(followingsPostsTask).Concat(myPostsTask),
                new PostEqualityComparer()
            );

            var random = new Random();
            var shuffledPosts = allPosts.OrderBy(_ => random.Next()).ToList();

            var viewModel = new TimeLineViewModel
            {
                FriendsPosts = friendsPostsTask,
                FollowingsPosts = followingsPostsTask,
                MyPosts = myPostsTask,
                Posts = shuffledPosts
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (userId is null)
                    return NotFound();

                var profile = await _profileService.GetProfileByUserIdAsync(userId);
                if (profile is null)
                    return NotFound();

                var newPost = new Post
                {
                    Content = model.Content,
                    ProfileId = profile.Id
                };

                await _feedService.AddPostAsync(newPost);
                return RedirectToAction("TimeLine");
            }
            return View(model);
        }
        public async Task<IActionResult> PostDetails(Guid? id)
        {
            if (id is null)
                return NotFound();

            var post = await _feedService.GetPostByIdAsync(id.Value);
            if (post is null)
                return NotFound();

            var comments = await _feedService.GetCommentsByPostIdAsync(id.Value);
            var likes = await _feedService.GetLikesByPostIdAsync(id.Value);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            bool isLikedByCurrentUser = likes.Any(like => like.Profile.ApplicationUserId == userId);

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                Comments = comments,
                Likes = likes,
                IsLikedByCurrentUser = isLikedByCurrentUser
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var post = await _feedService.GetPostByIdAsync(postId);
            if (post is null || post.Profile.ApplicationUserId != userId)
                return Forbid();

            await _feedService.DeletePostAsync(postId);
            return RedirectToAction("TimeLine");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(Guid postId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            var existingLike = await _feedService.GetLikeByPostAndProfileAsync(postId, profile.Id);

            if (existingLike is not null)
                await _feedService.RemoveLikeAsync(existingLike);
            else
            {
                await _feedService.AddLikeAsync(postId, profile.Id);
                await _profileService.AddLikeNotificationAsync(postId, profile.FirstName);
            }

            return RedirectToAction("PostDetails", new { id = postId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(Guid postId, string content)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "You can\'t add an empty comment";
                return RedirectToAction("PostDetails", new { id = postId });
            }

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile is null)
                return NotFound();

            await _feedService.AddCommentAsync(postId, profile.Id, content);

            await _profileService.AddCommentNotificationAsync(postId, profile.FirstName);

            TempData["SuccessMessage"] = "Comment added successfully";
            return RedirectToAction("PostDetails", new { id = postId });
        }
        [HttpGet]
        public IActionResult SearchResults(SearchResultsViewModel SearchResult)
        {
            return View(SearchResult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string? query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("TimeLine");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var profile = await _profileService.GetProfileByUserIdAsync(userId);

            var profiles = await _feedService.SearchProfilesAsync(query, profile.Id);

            return View("SearchResults", new SearchResultsViewModel { SearchResults = profiles });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var comment = await _feedService.GetCommentByIdAsync(commentId);

            if (comment is null || comment.Profile.ApplicationUserId != userId)
                return Forbid();

            await _feedService.DeleteCommentAsync(commentId);

            return RedirectToAction("PostDetails", new { id = comment.PostId });
        }
        [HttpGet]
        public async Task<IActionResult> EditPost(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var post = await _feedService.GetPostByIdAsync(id);
            if (post is null || post.Profile.ApplicationUserId != userId)
                return Forbid();

            var viewModel = new EditPostViewModel
            {
                Id = post.Id,
                Content = post.Content
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditPostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var post = await _feedService.GetPostByIdAsync(model.Id);
            if (post is null || post.Profile.ApplicationUserId != userId)
                return Forbid();

            post.Content = model.Content;
            await _feedService.UpdatePostAsync(post);

            return RedirectToAction("PostDetails", new { id = model.Id });
        }
        [HttpGet]
        public async Task<IActionResult> EditComment(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var comment = await _feedService.GetCommentByIdAsync(id);
            if (comment is null || comment.Profile.ApplicationUserId != userId)
                return Forbid();

            var viewModel = new EditCommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(EditCommentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (userId is null)
                return NotFound();

            var comment = await _feedService.GetCommentByIdAsync(model.Id);
            if (comment is null || comment.Profile.ApplicationUserId != userId)
                return Forbid();

            comment.Content = model.Content;
            await _feedService.UpdateCommentAsync(comment);

            return RedirectToAction("PostDetails", new { id = model.PostId });
        }
    }
}
