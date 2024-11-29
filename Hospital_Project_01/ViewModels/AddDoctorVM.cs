using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class AddDoctorVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!!")]
        [StringLength(40)]
        public string Name { get; set; } 


        [Range(25, 100, ErrorMessage = "Age must be between 25 and 100")]
        public int Age { get; set; }


        [MaxLength(50)]
        public string? Address { get; set; }

        [Range(1000, 100000, ErrorMessage = "Salary must be between 1,000 and 100,000")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Set the doctor's rank")]
        public DoctorsRank Rank { get; set; }

        [Required]
        public Shift Shift { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public string AspNetUsersId { get; set; } 

        public int DepartmentId { get; set; }
    }
}
