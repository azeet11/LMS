using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100, ErrorMessage = "User Name cannot be longer than 100 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "User Type is required")]
    [StringLength(50, ErrorMessage = "User Type cannot be longer than 50 characters")]
    public string UserType { get; set; } // Student or Staff
}