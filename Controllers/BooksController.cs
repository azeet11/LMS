// File: Controllers/BooksController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace LibraryManagementSystem;


public class BooksController : Controller
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    // GET: Books
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(string bookAuthor, string searchString, int? page)
    {
        if (_context.Books == null)
        {
            return Problem("Entity set 'LibraryContext.Books' is null.");
        }

        // Use LINQ to get list of authors.
        IQueryable<string> authorQuery = from b in _context.Books
                                         orderby b.Author
                                         select b.Author;

        var books = from b in _context.Books
                    select b;

        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!string.IsNullOrEmpty(bookAuthor))
        {
            books = books.Where(x => x.Author == bookAuthor);
        }

        int pageSize = 5;
        int pageNumber = (page ?? 1);

        var booksearchVM = new HomePageViewModel
        {
            Author = new SelectList(await authorQuery.Distinct().ToListAsync()),
            Books = books.ToPagedList(pageNumber, pageSize)
        };

        return View(booksearchVM);
    }


    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Author,Publisher,Year,Pages,Language")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Publisher,Year,Pages,Language")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

    // Add the Borrow action method
    [Authorize(Roles = "Member")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(int bookId)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var userBorrowingsCount = _context.Borrowings.Count(b => b.UserId == int.Parse(userId));

        if (userBorrowingsCount >= 3)
        {
            TempData["ErrorMessage"] = "You cannot borrow more than 3 books.";
            return RedirectToAction(nameof(Index));
        }

        var borrowing = new Borrowing
        {
            UserId = int.Parse(userId),
            BookId = bookId,
            BorrowDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(14) // Example return date
        };

        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
