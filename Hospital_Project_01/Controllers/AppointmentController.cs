using BLL.Interfaces;
using BLL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.ViewModels;
using System.Security.Claims;

namespace PL.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository,
            IDepartmentRepository departmentRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _departmentRepository = departmentRepository;
            _doctorRepository = doctorRepository;
        }


        #region Index, filters
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public IActionResult Index(int? patientId)
        {
            if (User.IsInRole("Patient"))
            {
                return RedirectToAction(nameof(FilterByPatientId));
            }

            IEnumerable<Appointment>? appointments = _appointmentRepository.GetUpcomingAppointments();
            return View(appointments);
        }


        [Authorize(Roles = "Admin, Doctor, Patient")]
        public IActionResult FilterByPatientId(int? id) 
        {
            if (User.IsInRole("Patient"))
            {
                int patientId = int.Parse(User.FindFirstValue("id"));
                return View(nameof(Index) , _appointmentRepository.GetAppointmentsForPatient(patientId));
            }

            if(id == null) return RedirectToAction(nameof(Index));
            
            return View(nameof(Index) , _appointmentRepository.GetAppointmentsForPatient(id.Value));
        }

        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult FilterByDate(DateTime? date)
        {
            if(date is null) return RedirectToAction(nameof(Index));

            var appointments = _appointmentRepository.GetAppointmentsByDate(date.Value);
            return View("Index", appointments); 
        }

        #endregion


        #region Details
        [Authorize(Roles = "Admin, Doctor, Patient")]
        public IActionResult Details(int appointmentId)
        {
            Appointment? appointment = _appointmentRepository.GetAppointmentWithDoctor(appointmentId); 
            
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        #endregion


        #region Process Appointment
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult ProcessAppointment (int  appointmentId)
        {
            Appointment? appointment = _appointmentRepository.Get(appointmentId);
            Patient? patient = _patientRepository.GetPatientWithMedicalRecords(appointment?.PatientId ?? 0);

            if (appointment == null || patient == null) return NotFound();

            ProcessAppointmentVM appointmentDetails = new ProcessAppointmentVM
            {
                Id = appointmentId,
                Date = appointment.Date,
                Finished = appointment.Finished,
                History = patient.History,
                PatientId = patient.Id,
                MedicalRecords = patient.MedicalRecords,
                PatientAge = patient.Age,
                PatientName = patient.Name
            };

            return View(appointmentDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult ProcessAppointment(ProcessAppointmentVM appointmentVM)
        {
            // Retrieve the appointment based on the ID
            var appointment = _appointmentRepository.Get(appointmentVM.Id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Update the Finished status
            appointment.Finished = appointmentVM.Finished;

            try
            {
                _appointmentRepository.Update(appointment);
                _appointmentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to process appointment: " + ex.Message);
                return View(appointmentVM);
            }
        }

        #endregion


        #region Edit
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var appointment = _appointmentRepository.GetAppointmentWithDoctor(id);

            if (appointment == null) return NotFound();

            var appointmentVM = new EditAppointmentVM
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId, // hidden in the form
                DepartmentId = appointment.Doctor?.DepartmentId ?? 0, 
                Date = appointment.Date ?? DateTime.Now
            };

            ViewBag.Departments = _departmentRepository.GetAll();
            return View(appointmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAppointment(EditAppointmentVM appointmentVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _departmentRepository.GetAll();
                return View("Edit", appointmentVM);
            }

            var appointment = _appointmentRepository.Get(appointmentVM.Id);
            if (appointment == null) return NotFound();

            appointment.DoctorId = appointmentVM.DoctorId;
            appointment.Date = appointmentVM.Date;

            try
            {
                _appointmentRepository.Update(appointment);
                _appointmentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region Add Appointment
        [Authorize(Roles = "Admin")]
        public IActionResult Add(AddAppointmentVM appointmentVM)
        {
            var depts = _departmentRepository.GetAll();
            ViewBag.Departments = depts;

            appointmentVM.Date = DateTime.Now;
            return View(appointmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAppointment(AddAppointmentVM appointmentVM)
        {
            if (!ModelState.IsValid)
            {
                var depts = _departmentRepository.GetAll();
                ViewBag.Departments = depts;
                return View(nameof(Add),appointmentVM);
            }

            if(_patientRepository.Get(appointmentVM.PatientId) is null)
            {
                ModelState.AddModelError("PatientId", "No patient registered with this ID");
                var depts = _departmentRepository.GetAll();
                ViewBag.Departments = depts;
                return View(nameof(Add), appointmentVM);
            } 

            Appointment entity = new Appointment() { 
                DoctorId = appointmentVM.DoctorId, 
                PatientId = appointmentVM.PatientId, 
                Date = appointmentVM.Date 
            };

            try
            {
                _appointmentRepository.Add(entity);
                _appointmentRepository.Save(); 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        public JsonResult GetDoctorsByDepartment(int departmentId)
        {
            var dept = _departmentRepository.GetDepartmentWithDoctors(departmentId);
            var doctorList = dept?.Doctors.Select(d => new { id = d.Id, name = d.Name }).ToList();
            return Json(doctorList);
        }
        #endregion


        #region Delete 
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var appointment = _appointmentRepository.Get(id); 
           
            if (appointment == null)
            {
                return NotFound();
            }
            
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Appointment appointment)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _appointmentRepository.Delete(appointment); // Delete the appointment
                _appointmentRepository.Save(); // Save changes to the database
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
