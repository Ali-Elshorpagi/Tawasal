using Microsoft.AspNetCore.Identity;

namespace Tawasal.Models
{
    public class ApplicationUser : IdentityUser // the default is string
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
    }
}
