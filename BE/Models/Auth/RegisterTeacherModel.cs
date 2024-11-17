using System.ComponentModel.DataAnnotations;

namespace BE.Models.Auth;

public class RegisterTeacherModel
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