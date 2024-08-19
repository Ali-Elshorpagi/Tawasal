using Microsoft.AspNetCore.Identity;
using Tawasal.Contexts;
using Tawasal.Models;
using Tawasal.Repositories.IRepositories;
using Tawasal.ViewModels;

namespace Tawasal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInMAnager;

        public AccountRepository(UserManager<ApplicationUser> userManager,
                                 //SignInManager<ApplicationUser> signInMAnager,
                                 ApplicationContext context)
        {
            _userManager = userManager;
            //_signInMAnager = signInMAnager;
            _context = context;
        }
        public string? GetIdByUsername(string username)
        {
            return _context.ApplicationUsers.FirstOrDefault(u => u.UserName == username)?.Id;
        }
        public ApplicationUser? GetUserById(string id)
        {
            return _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public async Task<(ApplicationUser?, IdentityResult)> Create(RegisterViewModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newProfile = new Profile
                    {
                        FirstName = model.UserName,
                    };
                    ApplicationUser newUser = new ApplicationUser
                    {
                        UserName = model.UserName,
                        PasswordHash = model.Password,
                        Email = model.Email,
                        Profile = newProfile,
                        //ProfileId = newProfile.Id
                    };

                    newProfile.ApplicationUser = newUser;
                    newProfile.ApplicationUserId = newUser.Id;

                    var result = await _userManager.CreateAsync(newUser, model.Password);

                    Console.WriteLine($"User creation result: {result.Succeeded}");
                    if (result.Errors.Any())
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }

                    if (!result.Succeeded)
                    {
                        return (null, result);
                    }

                    Console.WriteLine($"New User Id: {newUser.Id}");

                    if (string.IsNullOrEmpty(newUser.Id))
                    {
                        throw new Exception("User Id is null after user creation...");
                    }

                    //await _context.Profiles.AddAsync(newProfile);
                    //await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return (newUser, result);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(ex);
                    return (null, IdentityResult.Failed(new IdentityError
                    {
                        Description = ex.Message
                    }));
                }
            }
        }
    }
}
