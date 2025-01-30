using System.ComponentModel.DataAnnotations;

namespace BE.DTOs.DTOs.Auth.Requests;

public class RegisterDto
{
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}