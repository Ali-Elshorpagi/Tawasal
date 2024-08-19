using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tawasal.Models;

namespace Tawasal.Contexts
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Profile>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Post>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Notification>()
                .Property(n => n.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Like>()
                .Property(l => l.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<FriendRequest>()
                .Property(fr => fr.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Friend>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Follower>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Following>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Comment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Profile>()
                .HasOne(p => p.ApplicationUser)
                .WithOne(u => u.Profile)
                .HasForeignKey<ApplicationUser>(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasOne(p => p.Profile)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.Profile)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(p => p.FriendRequests)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
                .HasOne(f => f.ProfileOne)
                .WithMany(p => p.Friends)
                .HasForeignKey(f => f.ProfileIdOne)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
                .HasOne(f => f.ProfileTwo)
                .WithMany()
                .HasForeignKey(f => f.ProfileIdTwo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follower>()
                .HasOne(f => f.FollowerProfile)
                .WithMany()
                .HasForeignKey(f => f.FollowerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follower>()
                .HasOne(f => f.FollowedProfile)
                .WithMany(p => p.Followers)
                .HasForeignKey(f => f.FollowedProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Following>()
                .HasOne(f => f.FollowingProfile)
                .WithMany()
                .HasForeignKey(f => f.FollowingProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Following>()
                .HasOne(f => f.FollowedProfile)
                .WithMany(p => p.Followings)
                .HasForeignKey(f => f.FollowedProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(l => l.Profile)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.Profile)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
        }
    }
}
