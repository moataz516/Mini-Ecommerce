using ECommerceProject.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceProject.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            var response = new Login();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            // if user exist
            if (!ModelState.IsValid) return View(login);
            var user = await _userManager.FindByEmailAsync(login.EmailAddress);
            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, login.Password);
                if (password)
                {
                    // password correct, sign in 
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            // if user not found
            TempData["Error"] = "Wrong credentials. please try again";
            return View(login);
        }


        public IActionResult Register()
        {
            var response = new Register();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid) return View(register);
            var user = await _userManager.FindByEmailAsync(register.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(register);
            }
            var newUser = new ApplicationUser()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.FirstName + register.LastName,
                Email = register.EmailAddress,
                UserWallet = 100,
                
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, register.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
