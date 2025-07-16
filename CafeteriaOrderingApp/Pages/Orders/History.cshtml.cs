using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Pages.Orders
{
    public class HistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public HistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string EmployeeNumber { get; set; }

        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(EmployeeNumber))
            {
                ModelState.AddModelError("", "Employee Number is required.");
                return Page();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == EmployeeNumber);

            if (employee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return Page();
            }

            Orders = await _context.Orders
                .Where(o => o.EmployeeId == employee.Id)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Page();
        }
    }
}
