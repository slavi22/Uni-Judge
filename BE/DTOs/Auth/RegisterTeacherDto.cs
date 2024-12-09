using System.ComponentModel.DataAnnotations;

namespace BE.DTOs.Auth;

public class RegisterTeacherDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    public string Secret { get; set; }
}