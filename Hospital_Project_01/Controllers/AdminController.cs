using BLL.Interfaces;
using BLL.Repositories;
using PL.ViewModels;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(IAdminRepository adminRepository, UserManager<AppUser> userManager )
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var admins = _adminRepository.GetAll();
            return View(admins);
        }


        public IActionResult Dashboard()
        {
            return View();  
        }


        #region Add..tested
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AdminVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.AspNetUsersId);
            if (user is null)
            {
                ModelState.AddModelError("AspNetUsersId", "Please register the user with a system account before proceeding.");
                return View(model);
            }


            try
            {
                var admin = new Admin
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    AspNetUsersId = model.AspNetUsersId
                };

                _adminRepository.Add(admin);
                await _userManager.AddToRoleAsync(user, "Admin"); 

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(model);
            }
        }

        #endregion


        #region Edit..tested
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var admin = _adminRepository.Get(id);
            if (admin == null)
            {
                return NotFound();
            }

            var model = new AdminVM
            {
                Id = admin.Id,
                Name = admin.Name,
                Age = admin.Age,
                Address = admin.Address,
                AspNetUsersId = admin.AspNetUsersId
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var admin = _adminRepository.Get(model.Id);
                if (admin == null)
                {
                    return NotFound();
                }

                admin.Name = model.Name;
                admin.Age = model.Age;
                admin.Address = model.Address;
                admin.AspNetUsersId = model.AspNetUsersId;

                _adminRepository.Update(admin);
                _adminRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(model);
            }
        }

        #endregion


        #region Delete..tested
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var admin = _adminRepository.Get(id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByIdAsync(admin.AspNetUsersId));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(admin);
            }
        }
        #endregion


        #region Details..tested
        public IActionResult Details(int id)
        {
            var admin = _adminRepository.Get(id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        #endregion
    }

}
