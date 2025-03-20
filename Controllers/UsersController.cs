using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace LibraryManagementSystem;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly LibraryContext _context;

    public UsersController(LibraryContext context)
    {
        _context = context;
    }

    // GET: Users
    public async Task<IActionResult> Index(string userType, string searchString, int? page)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'LibraryContext.Users' is null.");
        }

        IQueryable<string> userTypeQuery = from b in _context.Users
                                         orderby b.UserType
                                         select b.UserType;


        var users = from b in _context.Users
                        select b;

        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!string.IsNullOrEmpty(userType))
        {
            users = users.Where(x => x.UserType == userType);
        }

        int pageSize = 5;
        int pageNumber = (page ?? 1);

        var UsersearchVM = new HomePageViewModel
        {
            UserType = new SelectList(await userTypeQuery.Distinct().ToListAsync()),
            Users = users.ToPagedList(pageNumber, pageSize)
        };

        return View(UsersearchVM);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Role,UserType")] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: Users/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Role,UserType")] User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                existingUser.UserType = user.UserType;

                // Only update the password if a new one is provided
                if (!string.IsNullOrEmpty(user.Password) && user.Password != "********")
                {
                    existingUser.Password = user.Password;
                }

                _context.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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
        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
