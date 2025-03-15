// File: Models/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class RegisterViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    public string UserType { get; set; } // Student or Staff
}