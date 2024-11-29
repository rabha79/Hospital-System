using System.ComponentModel.DataAnnotations;

public class DepartmentVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
    public string Name { get; set; } = null!;

    
    public int? HeadId { get; set; }
}
