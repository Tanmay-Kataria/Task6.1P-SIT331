using System;
using System.Text.Json.Serialization;
namespace robot_controller_api;

public class LoginModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}