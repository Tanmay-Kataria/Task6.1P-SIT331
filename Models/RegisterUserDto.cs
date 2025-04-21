using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace robot_controller_api;
public class RegisterUserDto
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName  { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
    public string? Description { get; set; }
    public string? Role        { get; set; }
}
