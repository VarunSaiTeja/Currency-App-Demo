﻿namespace CurrencyApp.Data.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}