using System.ComponentModel.DataAnnotations;

namespace Employee.Models;

public record Employee
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Position is required.")]
    [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
    public string Position { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
    public decimal Salary { get; init; }  
}