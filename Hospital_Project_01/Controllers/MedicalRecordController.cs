using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PL.ViewModels;

namespace PL.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;

        public MedicalRecordController(IMedicalRecordRepository medicalRecordRepository, IPatientRepository patientRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            this._patientRepository = patientRepository;
        }

        
        [Authorize(Roles = "Admin, Patient, Doctor")]
        public IActionResult Index(int? patientId)
        {            
            if (User.IsInRole("Patient"))
            {
                patientId = int.Parse(User.FindFirst("id").Value);
            }
            
            if(patientId == null ) return View();

            ViewBag.PatientId = patientId;
            return View(_medicalRecordRepository.GetPatientRecords(patientId.Value));
        }


        #region Details
        [HttpGet]
        [Authorize(Roles = "Admin, Patient, Doctor")]
        public IActionResult Details(int recordId) 
        { 
            MedicalRecord? record = _medicalRecordRepository.GetRecordWithDrugs(recordId);
            
            if(record is null) return NotFound();

            return View(record); 
        }
        #endregion


        #region Add record

        [HttpGet]
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Add(int patientId)
        {
            AddRecordVM vm = new AddRecordVM() { PatientId = patientId };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Add(AddRecordVM recordVM)
        {
            if (!ModelState.IsValid) return View(recordVM);

            var patient = _patientRepository.Get(recordVM.PatientId);

            if(patient == null)
            {
                ModelState.AddModelError("PatientId", "Patient ID not found.");
                return View(recordVM);
            }

            MedicalRecord record = new MedicalRecord()
            {
                PatientId = recordVM.PatientId,
                Diagnose = recordVM.Diagnose,
                Drugs = recordVM.Drugs
            };

            try
            {
                _medicalRecordRepository.Add(record);
                _medicalRecordRepository.Save();
                return RedirectToAction(nameof(Index), new { patientId = recordVM.PatientId });
            }
            catch (Exception ex) { return BadRequest(ModelState); }
        }

        #endregion


        #region Delete record
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Delete(int recordId) 
        {
            MedicalRecord? record = _medicalRecordRepository.GetRecordWithDrugs(recordId);

            if (record is null) return NotFound();

            return View(record);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Delete(MedicalRecord record)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _medicalRecordRepository.Delete(record);
                _medicalRecordRepository.Save();
                return RedirectToAction(nameof(Index), new { recordId = record.PatientId }); 
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion
    }
}
