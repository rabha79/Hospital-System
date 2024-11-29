using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class BillAddVM
    {
        [Required(ErrorMessage = "The bill amount is required.")]
        [Range(minimum: 0.0, maximum: 1000000.0 , ErrorMessage = "The amount must be between 0.0 and 100000000.0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "The bill date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "The Patient ID is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select whether the bill is paid.")]
        public bool Paid { get; set; } = false;
    }
}
