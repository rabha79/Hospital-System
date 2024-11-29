using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using PL.ViewModels;
using System.Security.Claims;

namespace PL.Controllers
{

    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly UserManager<AppUser> _userManager;
         
        public DoctorController(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository,
            IDepartmentRepository departmentRepository, UserManager<AppUser> userManager)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
        }


        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Index()
        {
            if (User.IsInRole("Doctor"))
            {
                int id = int.Parse(User.FindFirstValue("id"));
                return RedirectToAction(nameof(Schedule), new {id});
            }
            return View(_doctorRepository.GetAll());
        }


        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Schedule(int id) 
        {
            var doctor = _doctorRepository.Get(id);
            if (doctor == null) return NotFound();

            IEnumerable<Appointment>? schedule = _appointmentRepository.GetDailyAppointmentsForDoctor(id);
            var viewModel = new DoctorScheduleViewModel
            {
                Doctor = doctor,
                Appointments = schedule,
                TotalAppointments = schedule.Count(),
                FinishedAppointments = schedule.Count(a => a.Finished),
                PendingAppointments = schedule.Count(a => !a.Finished)
            };


            return View(viewModel);
        }


        #region Add . tested
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(AddDoctorVM? doctorVM)
        {
            // Retrieve any necessary data for the view, such as departments
            var departments = _departmentRepository.GetAll();
            ViewBag.Departments = departments;

            return doctorVM != null ? View(doctorVM) : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDoctor(AddDoctorVM doctorVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index) , doctorVM);

            var user = await _userManager.FindByIdAsync(doctorVM.AspNetUsersId);
            if(user is null)
            {
                ModelState.AddModelError("AspNetUsersId", "Please register the doctor with a system account before proceeding.");
                var departments = _departmentRepository.GetAll();
                ViewBag.Departments = departments;
                return View(doctorVM);
            }


            Doctor newDoctor = new Doctor
            {
                Name = doctorVM.Name,
                Age = doctorVM.Age,
                Address = doctorVM.Address,
                Salary = doctorVM.Salary,
                Rank = doctorVM.Rank,
                Shift = doctorVM.Shift,
                AspNetUsersId = doctorVM.AspNetUsersId,  
                DepartmentId = doctorVM.DepartmentId
            };

            try
            {
                _doctorRepository.Add(newDoctor);
                await _userManager.AddToRoleAsync(user, "Doctor");
                return RedirectToAction("Index");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }
        #endregion


        #region Edit . tested

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var doctor = _doctorRepository.Get(id);
            if (doctor == null) return NotFound();


            var doctorVM = new AddDoctorVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Age = doctor.Age,
                Address = doctor.Address,
                Salary = doctor.Salary,
                Rank = doctor.Rank,
                Shift = doctor.Shift,
                AspNetUsersId = doctor.AspNetUsersId,
                DepartmentId = doctor.DepartmentId
            };

            var departments = _departmentRepository.GetAll();
            ViewBag.Departments = departments;

            return View(doctorVM);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AddDoctorVM doctorVM)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentRepository.GetAll();
                ViewBag.Departments = departments;
                return View(doctorVM);
            }

            var doctor = _doctorRepository.Get(doctorVM.Id);
            if (doctor == null || doctor.AspNetUsersId != doctorVM.AspNetUsersId) return NotFound();

            doctor.Name = doctorVM.Name;
            doctor.Age = doctorVM.Age;
            doctor.Address = doctorVM.Address;
            doctor.Salary = doctorVM.Salary;
            doctor.Rank = doctorVM.Rank;
            doctor.Shift = doctorVM.Shift;
            doctor.DepartmentId = doctorVM.DepartmentId;

            try
            {
                _doctorRepository.Update(doctor);
                _doctorRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }


        #endregion


        #region Delete . tested
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var doctor = _doctorRepository.GetDoctorWithDepartment(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Doctor doctor)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                //The delete action is CASCADE 
                await _userManager.DeleteAsync(await _userManager.FindByIdAsync(doctor.AspNetUsersId));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion


        #region Details . tested
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id) 
        {
            var doctor = _doctorRepository.GetDoctorWithDepartment(id);
            if (doctor == null) return NotFound();  

            return View (doctor);
        }

        #endregion
    }
}
