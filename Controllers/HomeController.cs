// File: Controllers/HomeController.cs
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string bookAuthor, string searchString, int? page)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = HttpContext.Session.GetString("UserId");

            if (userRole == "Admin")
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (userRole == "Member" && !string.IsNullOrEmpty(userId))
            {
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

                if (TempData["ErrorMessage"] != null)
                {
                    ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                var borrowedBooks = _context.Borrowings
                    .Include(b => b.Book) // Include Book details
                    .Where(b => b.UserId == int.Parse(userId))
                    .ToList();

                var viewModel = new HomePageViewModel
                {
                    Author = new SelectList(await authorQuery.Distinct().ToListAsync()),
                    Books = books.ToPagedList(pageNumber, pageSize),
                    BookAuthor = bookAuthor,
                    SearchString = searchString,
                    BorrowedBooks = borrowedBooks.ToPagedList(pageNumber, pageSize)
                };

                // Clear existing model state errors
                ModelState.Clear();

                return View(viewModel);
            }

            return View("Login", new LoginViewModel());
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // Set user session or authentication cookie
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("UserType", user.UserType);

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync("CookieAuthentication", new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("AdminDashboard");
                    }
                    else if (user.Role == "Member")
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            // Clear user session or authentication cookie
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("CookieAuthentication");
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var totalBorrowedBooks = await _context.Borrowings.CountAsync();
            var totalOverdueBooks = await _context.Borrowings.CountAsync(b => b.ReturnDate < DateTime.Now);

            var topAdmins = await _context.Users
                .Where(u => u.Role == "Admin")
                .OrderBy(u => u.Id)
                .Take(3)
                .ToListAsync();

            var topOverdueBorrowers = await _context.Borrowings
                .Include(b => b.User)
                .Where(b => b.ReturnDate < DateTime.Now)
                .OrderBy(b => b.ReturnDate)
                .Take(3)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalBorrowedBooks = totalBorrowedBooks,
                TotalOverdueBooks = totalOverdueBooks,
                TopAdmins = topAdmins,
                TopOverdueBorrowers = topOverdueBorrowers
            };

            return View(viewModel);
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index");
        }

        // Add the Register action methods
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.UserName,
                    Email = model.Email,
                    Password = model.Password, // Note: Hash the password in a real application
                    Role = "Member", // Set role to Member by default
                    UserType = model.UserType
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
