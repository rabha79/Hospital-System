using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace PL.ViewModels
{
    public class AddAppointmentVM
    {
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
   
    }
}
