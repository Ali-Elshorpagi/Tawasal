namespace Tawasal.Models
{
    public class Friend
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ProfileIdOne { get; set; }
        public Profile ProfileOne { get; set; } = null!;
        public Guid ProfileIdTwo { get; set; }
        public Profile ProfileTwo { get; set; } = null!;
    }
}
