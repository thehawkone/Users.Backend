﻿namespace Users.Domain.Models;

public class User
{
    public const int MaxUserNameLength = 100;
    
    public Guid Id { get; set; }
    public string UserName {get; set;}
    public string PasswordHash {get; set;}
    public string Email {get; set;}
}