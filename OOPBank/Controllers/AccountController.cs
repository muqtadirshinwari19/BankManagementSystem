using System.Threading.Tasks;
using BankSystem.Models;
using BankSystem.Models.Identities;
using BankSystem.Services.Enums;
using BankSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BankSystem.Controllers;

namespace BankSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;

        public AccountController(UserManager<User> user, SignInManager<User> signInManager, RoleManager<UserRole> roleManager)
        {
            _userManager = user;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Registration()
        {

            return View();

        }

        public async Task<IActionResult> Register(RegistrationDTO register)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration", register);
            }

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = register.Name,
                Email = register.Email,
                PhoneNumber = register.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                //Check for raddio button status
                if (register.UserType == BankSystem.Services.Enums.UserTypeOption.Admin)
                {
                    // Create Admin Role
                    if (await _roleManager.FindByNameAsync(UserTypeOption.Admin.ToString()) is null)
                    {
                        UserRole role = new UserRole()
                        {
                            Id= Guid.NewGuid().ToString(),
                            Name = UserTypeOption.Admin.ToString()
                        };
                        await _roleManager.CreateAsync(role);
                      
                    }

                    //Assign Admin Role to the User
                    await _userManager.AddToRoleAsync(user, UserTypeOption.Admin.ToString());

                }
                else
                {
                    //Assign User Role to the User
                    await _userManager.AddToRoleAsync(user, UserTypeOption.User.ToString());
                }

                }
                return RedirectToAction(nameof(Index));

             
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Registration", register);
            
            
        }
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");

            return View(model);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ValidEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
