using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using CRUDContactManager.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {

        //private field to hold the injected service

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        //constructor to inject the service into the controller

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO) 
        {
            //check for validation errors
            if (ModelState.IsValid == false) 
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage).ToList();
                return View(registerDTO);
            }

            //create an instance of ApplicationUser and populate it with data from RegisterDTO
            ApplicationUser user = new ApplicationUser() { PersonName = registerDTO.PersonName, UserName = registerDTO.Email, Email = registerDTO.Email, PhoneNumber = registerDTO.Phone };

            //create the user in the database using UserManager
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //check for the role and assign it to the user
                if (registerDTO.UserType == Core.Enums.UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                    //create admin role if it doesn't exist
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                        //add the new user to the admin role
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                    }
                    else 
                    {
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                    }


                }

                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        //create User role if it doesn't exist 
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.User.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                        //add the new User to the User role
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                    }
                }

                //sign in the user
                await _signInManager.SignInAsync(user, isPersistent: true);

                return RedirectToAction("Index", "Persons");
            }
            else
            {
                ViewBag.Errors = result.Errors.Select(temp => temp.Description).ToList();
                return View(registerDTO);
            }

            
        }


        //login action methods
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnURL)
        {

            if (!ModelState.IsValid) 
            {

                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage).ToList();
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) 
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (await _userManager.IsInRoleAsync(user,UserTypeOptions.Admin.ToString())) 
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                ;

                if (!string.IsNullOrEmpty(ReturnURL) && Url.IsLocalUrl(ReturnURL))
                {
                    return LocalRedirect(ReturnURL);
                }
                    return RedirectToAction( nameof(PersonsController.Index), "Persons" );
            }

            else
            {
                ModelState.AddModelError("Login", "Invalid Email or Password.");
                return View(loginDTO);
            }


        }


        //logout action methods
        
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }


        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Logout")]
        public async Task<IActionResult> LogoutPost()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }



        //action method to check if email is already registered

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email) 
        {
             ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null) 
            {
                return Json(true); //valid email, not registered
            }
            else
            {
                return Json(false); //invalid email, already registered
            }

        }

    }
}
