using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace PL.ViewModels
{
    public class ProcessAppointmentVM
    {
        [Display(Name = "Appointment ID")]
        public int Id { get; set; }

        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        public bool Finished { get; set; }

        public DateTime? Date { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = null!;

        [Display(Name = "Patient Age")]
        public int? PatientAge { get; set; }

        public string? History { get; set; }

        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; } 

    }
}
