using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;
using System.Security.Cryptography.Xml;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DepartmentController(IDepartmentRepository departmentRepository, IDoctorRepository doctorRepository)
        {
            _departmentRepository = departmentRepository;
            _doctorRepository = doctorRepository;
        }

        public IActionResult Index()
        {
            return View(_departmentRepository.GetAll());
        }

        #region Add . tested
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(DepartmentVM department)
        {
            if (!ModelState.IsValid)
                return View(department);

            if(department.HeadId != null)
            {
                var doctor = _doctorRepository.Get(department.HeadId.Value);
                
                if(doctor == null)
                {
                    ModelState.AddModelError("HeadId", $"No doctor exist with this ID: {department.HeadId}");
                    return View(department);
                }
            }

            Department entity = new Department { Name = department.Name, HeadId = department.HeadId };  

            try
            {
                _departmentRepository.Add(entity);
                _departmentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Edit . tested
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentRepository.Get(id);
            if (department == null) return NotFound();

            var departmentVM = new DepartmentVM
            {
                Id = department.Id,
                Name = department.Name,
                HeadId = department.HeadId
            };

            return View(departmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentVM department)
        {
            if (!ModelState.IsValid)
                return View(department);

            if (department.HeadId != null)
            {
                var doctor = _doctorRepository.Get(department.HeadId.Value);
                if (doctor == null)
                {
                    ModelState.AddModelError("HeadId", $"No doctor exists with this ID: {department.HeadId}");
                    return View(department);
                }
            }

            try
            {
                var entity = new Department
                {
                    Id = department.Id,
                    Name = department.Name,
                    HeadId = department.HeadId
                };

                _departmentRepository.Update(entity);
                _departmentRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete . tested
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var department = _departmentRepository.Get(id);
            if (department == null) return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _departmentRepository.Delete(department);
                _departmentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Details . tested
        public IActionResult Details(int id)
        {
            var department = _departmentRepository.GetDepartmentWithHead(id);
            if (department == null) return NotFound();

            return View(department);
        }
        #endregion
    
    }
}

