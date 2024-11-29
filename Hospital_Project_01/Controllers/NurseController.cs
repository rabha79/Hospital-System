using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;
using PL.ViewModels;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NurseController : Controller
    {
        private readonly INurseRepository _nurseRepository;
        private readonly UserManager<AppUser> _userManager;

        public NurseController(INurseRepository nurseRepository, UserManager<AppUser> userManager)
        {
            _nurseRepository = nurseRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_nurseRepository.GetAll());
        }


        #region Add..tested
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddNurseVM nurse)
        {
            if (!ModelState.IsValid)
                return View(nurse);

            if (await _userManager.FindByIdAsync(nurse.AspNetUsersId) is null)
            {
                ModelState.AddModelError("AspNetUsersId", "Please register the nurse with a system account before proceeding.");
                return View(nurse);
            }

            Nurse entity = new Nurse
            {
                Name = nurse.Name,
                AspNetUsersId = nurse.AspNetUsersId,
                Age = nurse.Age,
                Address = nurse.Address,
                Shift = nurse.Shift,
                Salary = nurse.Salary
            };

            try
            {
                _nurseRepository.Add(entity);
                _nurseRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nurse);
            }
        }
        #endregion

        
        #region Edit..tested
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nurse = _nurseRepository.Get(id);
            if (nurse == null) return NotFound();

            AddNurseVM vM = new AddNurseVM
            {
                Id = nurse.Id,
                Salary = nurse.Salary,
                Shift = nurse.Shift,
                Address = nurse.Address ?? "",
                Age = nurse.Age,
                AspNetUsersId = nurse.AspNetUsersId,
                Name = nurse.Name
            };

            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AddNurseVM nurse)
        {
            if (!ModelState.IsValid) return View(nurse);


            Nurse entity = new Nurse
            {
                Id = nurse.Id,
                Salary = nurse.Salary,
                Shift = nurse.Shift,
                Address = nurse.Address,
                Age = nurse.Age,
                AspNetUsersId = nurse.AspNetUsersId,
                Name = nurse.Name
            };


            try
            {
                _nurseRepository.Update(entity);
                _nurseRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nurse);
            }
        }
        #endregion

        
        #region Delete..tesetd
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var nurse = _nurseRepository.Get(id);
            if (nurse == null) return NotFound();

            return View(nurse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Nurse nurse)
        {
            if (!ModelState.IsValid) return View(ModelState);

            try
            {
                _nurseRepository.Delete(nurse);
                _nurseRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(nurse);
            }
        }
        #endregion


        #region Details..tested
        public IActionResult Details(int id)
        {
            var nurse = _nurseRepository.Get(id);
            if (nurse == null) return NotFound();
            return View(nurse);
        }
        #endregion
    }
}

