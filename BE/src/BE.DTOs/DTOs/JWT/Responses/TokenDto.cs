namespace BE.DTOs.DTOs.JWT.Responses;

public class TokenDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}