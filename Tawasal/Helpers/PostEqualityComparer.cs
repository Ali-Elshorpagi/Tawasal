using Tawasal.Models;

namespace Tawasal.Helpers
{
    public class PostEqualityComparer : IEqualityComparer<Post>
    {
        public bool Equals(Post x, Post y) => x.Id == y.Id;
        public int GetHashCode(Post obj) => obj.Id.GetHashCode();
    }
}