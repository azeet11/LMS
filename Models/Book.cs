using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, ErrorMessage = "Author cannot be longer than 100 characters")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Publisher is required")]
    [StringLength(100, ErrorMessage = "Publisher cannot be longer than 100 characters")]
    public string Publisher { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(1000, 9999, ErrorMessage = "Year must be a valid year")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Pages are required")]
    [Range(1, int.MaxValue, ErrorMessage = "Pages must be a positive number")]
    public int Pages { get; set; }

    [Required(ErrorMessage = "Language is required")]
    [StringLength(50, ErrorMessage = "Language cannot be longer than 50 characters")]
    public string Language { get; set; }
}