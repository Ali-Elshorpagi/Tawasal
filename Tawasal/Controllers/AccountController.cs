using Microsoft.AspNetCore.Mvc;
using Tawasal.Services.IServices;
using Tawasal.ViewModels;

namespace Tawasal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _accountService.Register(model);

                if (result.Item1 != null && result.Item2.Succeeded)
                {
                    await _accountService.Login(result.Item1, false);
                    return RedirectToAction("TimeLine", "Feed", new { id = result.Item1.Id });
                }
                foreach (var error in result.Item2.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserByUsername(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username is incorrect");
                    return View(model);
                }

                var result = await _accountService.CheckPassword(user, model.Password);
                if (!result)
                {
                    ModelState.AddModelError("", "Password is incorrect");
                    return View(model);
                }
                await _accountService.Login(user, model.RememberMe);
                return RedirectToAction("TimeLine", "Feed", new { id = user.Id });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ChangeUsername()
        {
            var currentUser = User?.Identity?.Name;
            var model = new ChangeUsernameViewModel { CurrentUsername = currentUser! };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserByUsername(model.CurrentUsername);
                if (user is null)
                {
                    ModelState.AddModelError("", "Current username is incorrect");
                    return View(model);
                }

                var result = await _accountService.ChangeUsername(user, model.NewUsername);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Profile", new { id = user.Id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserByUsername(User?.Identity?.Name!);
                if (user is null)
                {
                    ModelState.AddModelError("", "User not found");
                    return View(model);
                }

                var result = await _accountService.ChangePassword(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Profile", new { id = user.Id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}
