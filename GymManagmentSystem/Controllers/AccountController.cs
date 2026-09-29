using GymManagmentSystem.BLL.ViewModels.AccountViewModels;
using GymManagmentSystem.Controllers;
using GymManagmentSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.APi.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        // get
        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User : {user.UserName} Is Signed In ");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User : {user.UserName} Is Locked Out");
                ModelState.AddModelError("InvalidLogin", "This Account Is Locked , Please Try Again");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

        }

        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> Logout()
        {
            await  _signInManager.SignOutAsync();
            return View(nameof(Login));
        }

        [HttpGet]

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
