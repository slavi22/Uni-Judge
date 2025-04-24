using System.ComponentModel.DataAnnotations;

namespace BE.DTOs.DTOs.Auth.Requests;

public class RegisterDto
{
    [Required] //not really needed since our prop is not marked as nullable
    [EmailAddress]
    public string Email { get; set; }
    [Required] //not really needed since our prop is not marked as nullable
    [DataType(DataType.Password)]
    //TODO: add min length and stuff
    public string Password { get; set; }
}