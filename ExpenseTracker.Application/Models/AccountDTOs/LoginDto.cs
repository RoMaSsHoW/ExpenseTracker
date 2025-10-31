using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.Models.AccountDTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; } = string.Empty;
}