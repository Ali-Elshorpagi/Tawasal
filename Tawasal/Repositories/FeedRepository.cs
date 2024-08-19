using Microsoft.EntityFrameworkCore;
using Tawasal.Contexts;
using Tawasal.Models;
using Tawasal.Repositories.IRepositories;

namespace Tawasal.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly ApplicationContext _context;
        public FeedRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task AddLikeAsync(Guid postId, Guid profileId)
        {
            var like = new Like
            {
                PostId = postId,
                ProfileId = profileId
            };
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }
        public async Task AddCommentAsync(Guid postId, Guid profileId, string content)
        {
            var comment = new Comment
            {
                PostId = postId,
                ProfileId = profileId,
                Content = content
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
        public async Task AddPostAsync(Post newPost)
        {
            if (newPost is null || string.IsNullOrEmpty(newPost.Content))
                throw new ArgumentNullException(nameof(newPost));

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<Post>> GetMyPostsByProfileIdAsync(Guid profileId)
        {
            return await _context.Posts
                                    .AsSplitQuery()
                                    .Include(p => p.Comments)
                                    .Include(p => p.Likes)
                                    .Include(p => p.Profile)
                                    .Where(p => p.ProfileId == profileId)
                                    .ToListAsync();
        }
        public async Task<ICollection<Post>> GetFriendsPostsByProfileIdAsync(Guid profileId)
        {
            var friendIds = await _context.Friends
                                    .Where(f => f.ProfileIdOne == profileId || f.ProfileIdTwo == profileId)
                                    .Select(f => f.ProfileIdOne == profileId ? f.ProfileIdTwo : f.ProfileIdOne)
                                    .ToListAsync();

            var posts = await _context.Posts
                                    .AsSplitQuery()
                                    .Include(p => p.Comments)
                                    .Include(p => p.Likes)
                                    .Include(p => p.Profile)
                                    .Where(p => friendIds.Contains(p.ProfileId))
                                    .ToListAsync();
            return posts;
        }
        public ICollection<Post> GetFollowersPostsByProfileId(Guid profileId)
        {
            var followerProfilesIds = _context.Followers
                                    .Where(f => f.FollowerProfileId == profileId)
                                    .Select(f => f.FollowerProfileId)
                                    .ToList();
            var posts = _context.Posts
                                    .AsSplitQuery()
                                    .Include(p => p.Comments)
                                    .Include(p => p.Likes)
                                    .Include(p => p.Profile)
                                    .Where(p => followerProfilesIds.Contains(p.ProfileId))
                                    .ToList();
            return posts;
        }
        public async Task<ICollection<Post>> GetFollowingPostsByProfileIdAsync(Guid profileId)
        {
            var followedProfileIds = await _context.Followings
                                    .Where(f => f.FollowingProfileId == profileId)
                                    .Select(f => f.FollowedProfileId)
                                    .ToListAsync();
            var posts = await _context.Posts
                                    .AsSplitQuery()
                                    .Include(p => p.Comments)
                                    .Include(p => p.Likes)
                                    .Include(p => p.Profile)
                                    .Where(p => followedProfileIds.Contains(p.ProfileId))
                                    .ToListAsync();
            return posts;
        }
        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            return await _context.Posts
                                .AsSplitQuery()
                                .Include(p => p.Comments)
                                .Include(p => p.Likes)
                                .Include(p => p.Profile)
                                .FirstOrDefaultAsync(p => p.Id == postId);
        }
        public async Task<ICollection<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _context.Comments
                .Include(c => c.Profile)
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }
        public async Task<ICollection<Like>> GetLikesByPostIdAsync(Guid postId)
        {
            return await _context.Likes
                .Include(l => l.Profile)
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }
        public async Task<Like?> GetLikeByPostAndProfileAsync(Guid postId, Guid profileId)
        {
            return await _context.Likes
                 .Include(l => l.Profile)
                 .FirstOrDefaultAsync(like => like.PostId == postId && like.ProfileId == profileId);
        }
        public async Task RemoveLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
        public async Task<ICollection<Profile>> SearchProfilesAsync(string query, Guid profileId)
        {
            return await _context.Profiles
                              .Where(p => (p.FirstName.Contains(query) ||
                              (p.LastName ?? string.Empty).Contains(query) ||
                              p.ApplicationUser.UserName!.Contains(query)) && p.Id != profileId)
                              .ToListAsync();
        }
        public async Task<Comment?> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments
             .Include(c => c.Profile)
             .FirstOrDefaultAsync(c => c.Id == commentId);
        }
        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment is not null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeletePostAsync(Guid postId)
        {
            var post = await _context.Posts
                        .Include(p => p.Comments)
                        .Include(p => p.Likes)
                        .FirstOrDefaultAsync(p => p.Id == postId);
            if (post is not null)
            {
                _context.Comments.RemoveRange(post.Comments);
                _context.Likes.RemoveRange(post.Likes);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
