using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new LibraryContext(
            serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Check if there are any users in the database
            if (!context.Users.Any())
            {
                // Add the default admin user
                var adminUser = new User
                {
                    Name = "Admin",
                    Email = "admin@admin.com",
                    Password = "Admin@123", // Note: Hash the password in a real application
                    Role = "Admin",
                    UserType = "Staff"
                };

                context.Users.Add(adminUser);
            }

            // Check if there are any books in the database
            if (!context.Books.Any())
            {
                // Add a list of 20 books
                var books = new List<Book>
                {
                    new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Publisher = "J.B. Lippincott & Co.", Year = 1960, Pages = 281, Language = "English" },
                    new Book { Title = "1984", Author = "George Orwell", Publisher = "Secker & Warburg", Year = 1949, Pages = 328, Language = "English" },
                    new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Publisher = "T. Egerton", Year = 1813, Pages = 279, Language = "English" },
                    new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons", Year = 1925, Pages = 180, Language = "English" },
                    new Book { Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers", Year = 1851, Pages = 635, Language = "English" },
                    new Book { Title = "War and Peace", Author = "Leo Tolstoy", Publisher = "The Russian Messenger", Year = 1869, Pages = 1225, Language = "English" },
                    new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", Publisher = "Little, Brown and Company", Year = 1951, Pages = 214, Language = "English" },
                    new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Publisher = "George Allen & Unwin", Year = 1937, Pages = 310, Language = "English" },
                    new Book { Title = "Fahrenheit 451", Author = "Ray Bradbury", Publisher = "Ballantine Books", Year = 1953, Pages = 194, Language = "English" },
                    new Book { Title = "Jane Eyre", Author = "Charlotte Brontë", Publisher = "Smith, Elder & Co.", Year = 1847, Pages = 500, Language = "English" },
                    new Book { Title = "Brave New World", Author = "Aldous Huxley", Publisher = "Chatto & Windus", Year = 1932, Pages = 311, Language = "English" },
                    new Book { Title = "Wuthering Heights", Author = "Emily Brontë", Publisher = "Thomas Cautley Newby", Year = 1847, Pages = 416, Language = "English" },
                    new Book { Title = "The Odyssey", Author = "Homer", Publisher = "Ancient Greece", Year = 1800, Pages = 541, Language = "English" },
                    new Book { Title = "Crime and Punishment", Author = "Fyodor Dostoevsky", Publisher = "The Russian Messenger", Year = 1866, Pages = 671, Language = "English" },
                    new Book { Title = "The Brothers Karamazov", Author = "Fyodor Dostoevsky", Publisher = "The Russian Messenger", Year = 1880, Pages = 824, Language = "English" },
                    new Book { Title = "The Divine Comedy", Author = "Dante Alighieri", Publisher = "Italy", Year = 1320, Pages = 798, Language = "English" },
                    new Book { Title = "The Iliad", Author = "Homer", Publisher = "Ancient Greece", Year = 1750, Pages = 704, Language = "English" },
                    new Book { Title = "Les Misérables", Author = "Victor Hugo", Publisher = "A. Lacroix, Verboeckhoven & Cie", Year = 1862, Pages = 1463, Language = "English" },
                    new Book { Title = "The Count of Monte Cristo", Author = "Alexandre Dumas", Publisher = "Penguin Classics", Year = 1844, Pages = 1276, Language = "English" },
                    new Book { Title = "Don Quixote", Author = "Miguel de Cervantes", Publisher = "Francisco de Robles", Year = 1605, Pages = 1072, Language = "English" }
                };

                context.Books.AddRange(books);
            }

            context.SaveChanges();
        }
    }
}
