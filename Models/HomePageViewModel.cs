using System.Collections.Generic;

namespace LibraryManagementSystem.Models
{
    public class HomePageViewModel
    {
        public List<Book> Books { get; set; }
        public List<Borrowing> BorrowedBooks { get; set; }
    }
}