using Microsoft.AspNetCore.Identity;
using Tawasal.Models;
using Tawasal.ViewModels;

namespace Tawasal.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        public Task<(ApplicationUser?, IdentityResult)> Create(RegisterViewModel model);
        public string? GetIdByUsername(string username);
        public ApplicationUser? GetUserById(string id);
    }
}
