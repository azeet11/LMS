using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        // Navigation property for Book
        public Book? Book { get; set; }

        // Navigation property for User
        public User? User { get; set; }

    }
}