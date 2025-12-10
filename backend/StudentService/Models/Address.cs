using System.ComponentModel.DataAnnotations;

namespace StudentService.Models;

public class Address
{
    [Required]
    public string Address1 { get; set; } = string.Empty;

    public string? Address2 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zipcode format")]
    public string Zipcode { get; set; } = string.Empty;
}
