﻿using Microsoft.AspNetCore.Identity;

namespace BE.Models.Auth;

public class AppUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}