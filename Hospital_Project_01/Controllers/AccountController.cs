using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Controllers;
using PL.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAdminRepository adminRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatientRepository patientRepository;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
            SignInManager<AppUser> signInManager, IAdminRepository adminRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.adminRepository = adminRepository;
            this.doctorRepository = doctorRepository;
            this.patientRepository = patientRepository;
        }

        

        #region Register..tested 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };


                if(await _userManager.FindByEmailAsync(user.UserName) is null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
                else ModelState.AddModelError(string.Empty, "This email already registered before.");

            }
            return View(model);
        }

        #endregion

        #region Login .. tested
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
                var search = await _userManager.FindByEmailAsync(model.Email);

                if (search != null && await _userManager.CheckPasswordAsync(search, model.Password))
                {
                    var roles = await _userManager.GetRolesAsync(search);
                    var claims = await _userManager.GetClaimsAsync(search);

                    if (roles.Contains("Admin"))
                    {
                        int id = adminRepository.GetByAspUsersId(search.Id).Id;
                        claims.Add(new Claim("id", id.ToString()));
                    }
                    else if (roles.Contains("Doctor"))
                    {
                        int id = doctorRepository.GetByAspUsersId(search.Id).Id;
                        claims.Add(new Claim("id", id.ToString()));
                    }
                    else if (roles.Contains("Patient"))
                    {
                        int id = patientRepository.GetByAspUsersId(search.Id).Id;
                        claims.Add(new Claim("id", id.ToString()));
                    }


                    await _signInManager.SignInWithClaimsAsync(search,  false, claims);
                    return RedirectToAction(nameof(HomeController.Index), "Home");

                }
                ModelState.AddModelError(string.Empty, "Invalid input: make sure of your email and password");
            }
            return View(model);
        }

        #endregion

        #region Logout
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchEmail)
        {
            if (!string.IsNullOrEmpty(searchEmail))
            {
                var user = await _userManager.FindByEmailAsync(searchEmail);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var viewModel = new UserDetailsViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = roles.ToList()
                    };
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "User not found.";
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}


