﻿using CurrencyApp.Data.Entities;

namespace CurrencyApp.Application.Features.Users.GetUser;

public class GetUserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<UserRole> Roles { get; set; }
}
