using Tawasal.Models;
using Tawasal.Repositories.IRepositories;
using Tawasal.Services.IServices;

namespace Tawasal.Services
{
    public class FeedService : IFeedService
    {

        private readonly IFeedRepository _feedRepository;
        public FeedService(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }
        public async Task<ICollection<Post>> GetMyPostsByProfileIdAsync(Guid profileId)
        {
            return await _feedRepository.GetMyPostsByProfileIdAsync(profileId);
        }
        public async Task<ICollection<Post>> GetFriendsPostsByProfileIdAsync(Guid profileId)
        {
            return await _feedRepository.GetFriendsPostsByProfileIdAsync(profileId);
        }
        public ICollection<Post> GetFollowersPostsByProfileId(Guid profileId)
        {
            return _feedRepository.GetFollowersPostsByProfileId(profileId);
        }
        public async Task<ICollection<Post>> GetFollowingPostsByProfileIdAsync(Guid profileId)
        {
            return await _feedRepository.GetFollowingPostsByProfileIdAsync(profileId);
        }
        public async Task AddPostAsync(Post newPost)
        {
            await _feedRepository.AddPostAsync(newPost);
        }
        public async Task AddLikeAsync(Guid postId, Guid profileId)
        {
            await _feedRepository.AddLikeAsync(postId, profileId);
        }
        public async Task AddCommentAsync(Guid postId, Guid profileId, string content)
        {
            await _feedRepository.AddCommentAsync(postId, profileId, content);
        }
        public async Task<Post?> GetPostByIdAsync(Guid postId)
        {
            return await _feedRepository.GetPostByIdAsync(postId);
        }
        public async Task<ICollection<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await _feedRepository.GetCommentsByPostIdAsync(postId);
        }
        public async Task<ICollection<Like>> GetLikesByPostIdAsync(Guid postId)
        {
            return await _feedRepository.GetLikesByPostIdAsync(postId);
        }
        public async Task<Like?> GetLikeByPostAndProfileAsync(Guid postId, Guid profileId)
        {
            return await _feedRepository.GetLikeByPostAndProfileAsync(postId, profileId);
        }
        public async Task RemoveLikeAsync(Like like)
        {
            await _feedRepository.RemoveLikeAsync(like);
        }
        public async Task<ICollection<Profile>> SearchProfilesAsync(string query, Guid profileId)
        {
            return await _feedRepository.SearchProfilesAsync(query, profileId);
        }
        public async Task<Comment?> GetCommentByIdAsync(Guid commentId)
        {
            return await _feedRepository.GetCommentByIdAsync(commentId);
        }
        public async Task DeleteCommentAsync(Guid commentId)
        {
            await _feedRepository.DeleteCommentAsync(commentId);
        }
        public async Task DeletePostAsync(Guid postId)
        {
            await _feedRepository.DeletePostAsync(postId);
        }
        public async Task UpdatePostAsync(Post post)
        {
            await _feedRepository.UpdatePostAsync(post);
        }
        public async Task UpdateCommentAsync(Comment comment)
        {
            await _feedRepository.UpdateCommentAsync(comment);
        }
    }
}
