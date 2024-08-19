using Tawasal.Models;

namespace Tawasal.Services.IServices
{
    public interface IFeedService
    {
        public Task AddPostAsync(Post newPost);
        public Task AddLikeAsync(Guid postId, Guid profileId);
        public Task AddCommentAsync(Guid postId, Guid profileId, string content);
        public Task<ICollection<Post>> GetMyPostsByProfileIdAsync(Guid profileId);
        public Task<ICollection<Post>> GetFriendsPostsByProfileIdAsync(Guid profileId);
        public ICollection<Post> GetFollowersPostsByProfileId(Guid profileId);
        public Task<ICollection<Post>> GetFollowingPostsByProfileIdAsync(Guid profileId);
        public Task<Post?> GetPostByIdAsync(Guid postId);
        public Task<ICollection<Comment>> GetCommentsByPostIdAsync(Guid postId);
        public Task<ICollection<Like>> GetLikesByPostIdAsync(Guid postId);
        public Task<Like?> GetLikeByPostAndProfileAsync(Guid postId, Guid profileId);
        public Task RemoveLikeAsync(Like like);
        public Task<ICollection<Profile>> SearchProfilesAsync(string query, Guid profileId);
        public Task<Comment?> GetCommentByIdAsync(Guid commentId);
        public Task DeleteCommentAsync(Guid commentId);
        public Task DeletePostAsync(Guid postId);
        public Task UpdatePostAsync(Post post);
        public Task UpdateCommentAsync(Comment comment);
    }
}
