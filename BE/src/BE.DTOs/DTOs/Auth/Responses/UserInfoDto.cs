namespace BE.DTOs.DTOs.Auth.Responses;

public class UserInfoDto
{
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}