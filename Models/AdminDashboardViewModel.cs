namespace LibraryManagementSystem.Models;

public class AdminDashboardViewModel
{
    public int TotalBorrowedBooks { get; set; }
    public int TotalOverdueBooks { get; set; }
    public List<User> TopAdmins { get; set; }
    public List<Borrowing> TopOverdueBorrowers { get; set; }
}