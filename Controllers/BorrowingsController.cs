using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.Extensions;

namespace LibraryManagementSystem;


public class BorrowingsController : Controller
{
    private readonly LibraryContext _context;

    public BorrowingsController(LibraryContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Member")]
    // GET: Borrowings
    public async Task<IActionResult> Index(string searchString,int? page)
    {
        if (_context.Borrowings == null)
        {
            return Problem("Entity set 'LibraryContext.Borrowings' is null.");
        }

        var borrowing = from b in _context.Borrowings.Include(b => b.Book)
                    select b;

        if (!string.IsNullOrEmpty(searchString))
        {
            borrowing = borrowing.Where(s => s.Book!.Title!.ToUpper().Contains(searchString.ToUpper()));
        }

        int pageSize = 5;
        int pageNumber = (page ?? 1);

        var bookborrowedsearchVM = new HomePageViewModel
        {
            BorrowedBooks = borrowing.ToPagedList(pageNumber, pageSize)
        };

        return View(bookborrowedsearchVM);
    }

    // GET: Borrowings/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (borrowing == null)
        {
            return NotFound();
        }

        return View(borrowing);
    }

    // GET: Borrowings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Borrowings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,UserId,BookId,BorrowDate,ReturnDate")] Borrowing borrowing)
    {
        if (ModelState.IsValid)
        {
            _context.Add(borrowing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(borrowing);
    }

    // GET: Borrowings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings.Include(b => b.Book).FirstOrDefaultAsync(b => b.Id == id);
        if (borrowing == null)
        {
            return NotFound();
        }
        return View(borrowing);
    }

    // POST: Borrowings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BookId,BorrowDate,ReturnDate,Book")] Borrowing borrowing)
    {
        if (id != borrowing.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(borrowing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(borrowing.Id))
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
        return View(borrowing);
    }

    // GET: Borrowings/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (borrowing == null)
        {
            return NotFound();
        }

        return View(borrowing);
    }

    // POST: Borrowings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing != null)
        {
            _context.Borrowings.Remove(borrowing);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BorrowingExists(int id)
    {
        return _context.Borrowings.Any(e => e.Id == id);
    }

    [Authorize(Roles = "Member")]
    public async Task<IActionResult> Borrow(int? page)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var borrowedBooks = await _context.Borrowings
            .Include(b => b.Book)
            .Where(b => b.UserId == int.Parse(userId))
            .ToListAsync();

        var viewModel = new HomePageViewModel
        {
            BorrowedBooks = borrowedBooks.ToPagedList(page ?? 1, 5)
        };

        return View(viewModel);
    }

    [Authorize(Roles = "Member")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var borrowing = await _context.Borrowings.FindAsync(id);
        if (borrowing == null)
        {
            TempData["ErrorMessage"] = "Borrowing record not found.";
            return RedirectToAction("Borrow", "Borrowings");
        }

        _context.Borrowings.Remove(borrowing);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Book returned successfully.";
        return RedirectToAction("Borrow", "Borrowings");
    }
}
