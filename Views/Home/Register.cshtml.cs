using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views.Home
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel RegisterData { get; set; }

        private readonly LibraryContext _context;

        public RegisterModel(LibraryContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = RegisterData.UserName,
                    Email = RegisterData.Email,
                    Password = RegisterData.Password, // Note: Hash the password in a real application
                    UserType = RegisterData.UserType
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToPage("Login");
            }

            return Page();
        }
    }
}
