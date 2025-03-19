using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace LibraryManagementSystem.Models;

public class HomePageViewModel
{
    public SelectList Author { get; set; }
    public IPagedList<Book> Books { get; set; }
    public string BookAuthor { get; set; }
    public string SearchString { get; set; }
    public List<Borrowing> BorrowedBooks { get; set; }
}
