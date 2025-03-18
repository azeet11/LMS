using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Models;

public class HomePageViewModel
{
    public SelectList? Author { get; set; }
    public List<Book> Books { get; set; }
    public string? BookAuthor { get; set; }
    public string? SearchString { get; set; }
    public List<Borrowing> BorrowedBooks { get; set; }
}