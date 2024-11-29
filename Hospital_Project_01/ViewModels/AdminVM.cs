using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class AdminVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(40, ErrorMessage = "Name cannot exceed 40 characters")]
        public string Name { get; set; }

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int? Age { get; set; }

        [StringLength(50, ErrorMessage = "Address cannot exceed 50 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "AppUser is required")]
        public string? AspNetUsersId { get; set; }
    }
}

