using Microsoft.AspNetCore.Identity;
using Tawasal.Models;
using Tawasal.ViewModels;

namespace Tawasal.Services.IServices
{
    public interface IAccountService
    {
        public Task<(ApplicationUser?, IdentityResult)> Register(RegisterViewModel model);
        public Task Login(ApplicationUser user, bool persistent);
        public Task<ApplicationUser> GetUserByUsername(string username);
        public Task<bool> CheckPassword(ApplicationUser user, string password);
        public Task Logout();
        public Task<IdentityResult> ChangeUsername(ApplicationUser user, string newUsername);
        public Task<IdentityResult> ChangePassword(ApplicationUser user, string currentPassword, string newPassword);
    }
}
