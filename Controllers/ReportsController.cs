using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly LibraryContext _context;

        public ReportsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Reports/BorrowedBooks
        public IActionResult BorrowedBooks()
        {
            var borrowedBooks = _context.Borrowings.ToList();
            return View(borrowedBooks);
        }

        // GET: Reports/OverduedBooks
        public IActionResult OverduedBooks()
        {
            var overduedBooks = _context.Borrowings.Where(b => b.ReturnDate < DateTime.Now).ToList();
            return View(overduedBooks);
        }

        // GET: Reports/PopularBooks
        public IActionResult PopularBooks()
        {
            var popularBooks = _context.Borrowings
                .GroupBy(b => b.BookId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .ToList();
            return View(popularBooks);
        }
    }
}
