using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class PatientCreateVM
    {
        [Required(ErrorMessage = "Patient name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Patient age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Patient's user ID is required")]
        public string AspNetUsersId { get; set; }


        public string? Address { get; set; }

        public string? History { get; set; }
    }
}
