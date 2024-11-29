using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PL.ViewModels
{
    public class AddRecordVM
    {
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Diagnose field is required")]
        public string Diagnose { get; set; }

        public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
    }
}
