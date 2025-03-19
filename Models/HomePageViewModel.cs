using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace LibraryManagementSystem.Models;

public class HomePageViewModel
{
    public SelectList Author { get; set; }
    public IPagedList<Book> Books { get; set; }
    public string BookAuthor { get; set; }
    public string SearchString { get; set; }
    public IPagedList<Borrowing> BorrowedBooks { get; set; }
    public IPagedList<User> Users { get; set; }
    public SelectList UserType { get; set; }
    public string UsersType { get; set; }
}
