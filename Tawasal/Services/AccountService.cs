using Microsoft.AspNetCore.Identity;
using Tawasal.Models;
using Tawasal.Repositories.IRepositories;
using Tawasal.Services.IServices;
using Tawasal.ViewModels;

namespace Tawasal.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        public AccountService(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
        }
        public async Task<(ApplicationUser?, IdentityResult)> Register(RegisterViewModel model)
        {
            return await _accountRepository.Create(model);
        }
        public async Task Login(ApplicationUser user, bool persistent)
        {
            //var identity = new ClaimsIdentity(await _userManager.GetClaimsAsync(user), "Login");
            //identity.AddClaim(new Claim("ProfileId", user.ProfileId.ToString()));

            //var principal = new ClaimsPrincipal(identity);

            await _signInManager.SignInAsync(user, isPersistent: persistent);
        }
        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> ChangeUsername(ApplicationUser user, string newUsername)
        {
            user.UserName = newUsername;
            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> ChangePassword(ApplicationUser user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
