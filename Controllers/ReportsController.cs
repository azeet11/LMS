using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers;

public class ReportsController : Controller
{
    private readonly LibraryContext _context;

    public ReportsController(LibraryContext context)
    {
        _context = context;
    }

    // GET: Reports/Reports
    public IActionResult Index()
    {
        return View();
    }
    // GET: Reports/BorrowedBooksPartial
    public async Task<IActionResult> BorrowedBooksPartial()
    {
        var borrowedBooks = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .ToListAsync();
        return PartialView("_BorrowedBooksPartial", borrowedBooks);
    }

    // GET: Reports/OverduedBooksPartial
    public async Task<IActionResult> OverduedBooksPartial()
    {
        var overduedBooks = await _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.User)
            .Where(b => b.ReturnDate < DateTime.Now)
            .ToListAsync();
        return PartialView("_OverduedBooksPartial", overduedBooks);
    }

    public async Task<IActionResult> PopularBooksPartial()
    {
        var popularBooks = await _context.Borrowings
            .Include(b => b.Book)
            .GroupBy(b => b.BookId)
            .OrderByDescending(g => g.Count())
            .Select(g => new { Book = g.First().Book, Count = g.Count() })
            .ToListAsync();
        return PartialView("_PopularBooksPartial", popularBooks);
    }

    // GET: Reports/TotalBorrowedBooks
    public async Task<int> TotalBorrowedBooks()
    {
        return await _context.Borrowings.CountAsync();
    }

    // GET: Reports/TotalOverdueBooks
    public async Task<int> TotalOverdueBooks()
    {
        return await _context.Borrowings.CountAsync(b => b.ReturnDate < DateTime.Now);
    }

    // GET: Reports/LoadPartialView
    public async Task<IActionResult> LoadPartialView(string partialView)
    {
        if (partialView == "BorrowedBooksPartial")
        {
            return await BorrowedBooksPartial();
        }
        else if (partialView == "OverduedBooksPartial")
        {
            return await OverduedBooksPartial();
        }
        else if (partialView == "PopularBooksPartial")
        {
            return await PopularBooksPartial();
        }
        return BadRequest();
    }
}
