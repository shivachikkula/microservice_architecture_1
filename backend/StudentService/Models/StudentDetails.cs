using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentService.Models;

[Table("Students")]
public class StudentDetails
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    // Address 1
    [Required]
    public string Address1_Address1 { get; set; } = string.Empty;
    public string? Address1_Address2 { get; set; }
    [Required]
    public string Address1_City { get; set; } = string.Empty;
    [Required]
    public string Address1_State { get; set; } = string.Empty;
    [Required]
    public string Address1_Zipcode { get; set; } = string.Empty;

    // Address 2
    [Required]
    public string Address2_Address1 { get; set; } = string.Empty;
    public string? Address2_Address2 { get; set; }
    [Required]
    public string Address2_City { get; set; } = string.Empty;
    [Required]
    public string Address2_State { get; set; } = string.Empty;
    [Required]
    public string Address2_Zipcode { get; set; } = string.Empty;

    // Address 3
    [Required]
    public string Address3_Address1 { get; set; } = string.Empty;
    public string? Address3_Address2 { get; set; }
    [Required]
    public string Address3_City { get; set; } = string.Empty;
    [Required]
    public string Address3_State { get; set; } = string.Empty;
    [Required]
    public string Address3_Zipcode { get; set; } = string.Empty;

    // Address 4
    [Required]
    public string Address4_Address1 { get; set; } = string.Empty;
    public string? Address4_Address2 { get; set; }
    [Required]
    public string Address4_City { get; set; } = string.Empty;
    [Required]
    public string Address4_State { get; set; } = string.Empty;
    [Required]
    public string Address4_Zipcode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
