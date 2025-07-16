using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Pages.Admin
{
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrdersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.Employee)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
        
    }
}
