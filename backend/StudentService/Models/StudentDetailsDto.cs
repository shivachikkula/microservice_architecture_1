using System.ComponentModel.DataAnnotations;

namespace StudentService.Models;

public class StudentDetailsDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string DOB { get; set; } = string.Empty;

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public Address Address1 { get; set; } = new Address();

    [Required]
    public Address Address2 { get; set; } = new Address();

    [Required]
    public Address Address3 { get; set; } = new Address();

    [Required]
    public Address Address4 { get; set; } = new Address();
}
