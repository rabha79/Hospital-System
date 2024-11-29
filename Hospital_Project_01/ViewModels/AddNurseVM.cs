using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class AddNurseVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Age is required.")]
        [Range(minimum: 18 , maximum: 100)]
        public int Age { get; set; }        
        
        [Required(ErrorMessage = "Salary is required.")]
        [Range(minimum: 0 , maximum: 100000)]
        public decimal Salary { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Shift is required")]
        public Shift Shift { get; set; }

        [MaxLength(450)]
        [Required(ErrorMessage = "User ID is required")]
        public string AspNetUsersId { get; set; }
    }
}
