using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDContactManager.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {

        //private field to hold the injected service

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //constructor to inject the service into the controller

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
    }
}
