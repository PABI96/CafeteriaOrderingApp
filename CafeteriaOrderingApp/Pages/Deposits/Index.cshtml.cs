using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace CafeteriaOrderingApp.Pages.Deposits
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string EmployeeNumber { get; set; }

        [BindProperty]
        [Range(1, double.MaxValue, ErrorMessage = "Deposit amount must be greater than zero.")]
        public decimal DepositAmount { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == EmployeeNumber);

            if (employee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return Page();
            }

            // Get current month/year
            var now = DateTime.Now;
            var currentYearMonth = new DateTime(now.Year, now.Month, 1);

            // If it's a new month, reset tracking
            if (employee.LastDepositMonth.Year != now.Year || employee.LastDepositMonth.Month != now.Month)
            {
                employee.LastDepositMonth = currentYearMonth;
                // Reset bonus logic by starting fresh
                employee.Balance = 0;
            }

            // Calculate how many full R250 units in this deposit
            int fullUnits = (int)(DepositAmount / 250);

            // Bonus logic
            decimal bonus = fullUnits * 500;

            employee.Balance += DepositAmount + bonus;

            // Update LastDepositMonth
            employee.LastDepositMonth = currentYearMonth;

            await _context.SaveChangesAsync();

            Message = $"Deposit successful. Bonus: R{bonus}. New Balance: R{employee.Balance}";

            return Page();
        }
        
    }
}
