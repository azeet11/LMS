using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class Borrowing
{
    private const float DailyFineRate = 10f;
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Book ID is required")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Borrow Date is required")]
    [DataType(DataType.Date)]
    public DateTime BorrowDate { get; set; }

    [Required(ErrorMessage = "Return Date is required")]
    [DataType(DataType.Date)]
    public DateTime ReturnDate { get; set; }

    public float? Fine { 
        get{
            var currentDate = DateTime.Now;
            if (currentDate > ReturnDate)
            {
                var overdueDays = (currentDate - ReturnDate).Days;
                return overdueDays * DailyFineRate;
            }
            else
            {
                return 0;
            }
        }
    }
    // Navigation property for Book
    public Book? Book { get; set; }

    // Navigation property for User
    public User? User { get; set; }
    public void CalculateFine()
    {
        
    }
}