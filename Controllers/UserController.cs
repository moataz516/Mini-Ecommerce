using ECommerceProject.Interfaces;
using ECommerceProject.Models;
using ECommerceProject.Models.Data;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECommerceProject.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _host;


        public UserController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userManager , IUserRepository userRepository, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment host)
        {
            _roleMgr = roleMgr;
            _userManager = userManager;
            _userRepository = userRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _host = host;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            return View(users);
        }


        
        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //var user = await _userRepository.GetDetails(id);
            var user = await _userRepository.GetUserById(id);

            if (user == null) return View("Error");

            //var editUserViewModel = new EditUserViewModel()
            //{
            //    Id = user.Id,
            //    FirstName = user.FirstName,
            //    UserName = user.UserName,
            //    ProfileImageUrl = user.ProfileImageUrl,
            //    City = user.City,
            //    State = user.State,
            //};

            if (user == null) return View("Error");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(string id ,[Bind("Id,UserName,Email,FirstName,LastNam,City,State,PhoneNumber,ProfileImageUrl,Image")] ApplicationUser euser)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View("Edit");
            }
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = _context.ApplicationUsers.Where(x => x.Id == curUser).FirstOrDefault();
            //var user = await _userRepository.GetByIdNoTracking(euser.Id);



            // ------------------------------ image ----------------------------------
            //if (euser.Image != null)
            //{
            //    var root = _host.WebRootPath;
            //    var fileName = Guid.NewGuid().ToString() + "_" + euser.Image.FileName;
            //    var path = Path.Combine(root + "/image/user_photo/", fileName);
            //    using (var fileStream = new FileStream(path, FileMode.Create))
            //    {
            //        await euser.Image.CopyToAsync(fileStream);
            //    }
            //    euser.ProfileImageUrl = fileName;
            //    _context.Update(euser);
            //    await _context.SaveChangesAsync();
            //    //_userRepository.Update(euser);
            //    return RedirectToAction("Index", "Home");
            //}


            ApplicationUser app = new ApplicationUser();

            app.Id = curUser;
            app.UserName = euser.UserName;
            app.FirstName = euser.FirstName;
            app.LastName = euser.LastName;
            app.Email = euser.Email;
            app.PhoneNumber = euser.PhoneNumber;
            app.City = euser.City;
            app.State = euser.State;


            _context.Update(app);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }







        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await _userRepository.GetDetails(id);
            if (user == null) return View("Error");



            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditUser([Bind("Id,UserName,Email,FirstName,LastName,City,State,PhoneNumber,ProfileImageUrl")] ApplicationUser euser , IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View("Edit");
            }
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = _context.Users.SingleOrDefault(x => x.Id == curUser);

            if (Image != null)
            {

                var root = _host.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                var path = Path.Combine(root + "/image/user_photo/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                user.ProfileImageUrl = fileName;
            }

            user.UserName = euser.UserName;
            user.Email = euser.Email;
            user.FirstName = euser.FirstName;
            user.LastName = euser.LastName;
            user.City = euser.City;
            user.State = euser.State;
            user.PhoneNumber = euser.PhoneNumber;


            if (user == null) { return View("Error"); }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }


  

        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }

      [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([Bind("OldPassword,NewPassword,ConfirmPassword")]ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
            var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userManager.FindByIdAsync(curUser);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    TempData["Success"] = "password Changed  successfully";
                    return View();
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                
            }
            return View(model);
        }



        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this._userRepository.GetUserById(id);
            if (user == null) return NotFound();
            _userRepository.Delete(user);
            return Redirect("Index");
        }




        public async Task<IActionResult> Order()
        {
            var cuUser = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userManager.FindByIdAsync(cuUser);

           var order = await _context.Orders.Where(x => x.UserId == user.Id).ToListAsync();

            return View(order);
        }

        public async Task<IActionResult> Orders()
        {

            var order = await _context.Orders.ToListAsync();

            return View(order);
        }

    }
}
