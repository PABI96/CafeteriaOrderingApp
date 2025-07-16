using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaOrderingApp.Pages.Orders
{
    public class PlaceOrderModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PlaceOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string EmployeeNumber { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new(); // MenuItemId → Quantity

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MenuItems = await _context.MenuItems
                .Include(m => m.Restaurant)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(EmployeeNumber))
            {
                ModelState.AddModelError("", "Employee number is required.");
                return await ReloadMenu();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == EmployeeNumber);

            if (employee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return await ReloadMenu();
            }

            // Get selected items
            var selectedItems = Quantities
                .Where(q => q.Value > 0)
                .Select(q => new
                {
                    MenuItem = _context.MenuItems.FirstOrDefault(m => m.Id == q.Key),
                    Quantity = q.Value
                })
                .Where(x => x.MenuItem != null)
                .ToList();

            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "No items selected.");
                return await ReloadMenu();
            }

            decimal total = selectedItems.Sum(i => i.MenuItem.Price * i.Quantity);

            if (employee.Balance < total)
            {
                ModelState.AddModelError("", $"Insufficient balance. Total: R{total}, Available: R{employee.Balance}");
                return await ReloadMenu();
            }

            // Deduct balance
            employee.Balance -= total;

            // Create order
            var order = new Order
            {
                EmployeeId = employee.Id,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Pending"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order items
            foreach (var item in selectedItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItem.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfOrder = item.MenuItem.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();

            // Optional: Show success message or redirect
            return RedirectToPage("/Orders/Confirmation", new { id = order.Id });
        }

        private async Task<IActionResult> ReloadMenu()
        {
            MenuItems = await _context.MenuItems.Include(m => m.Restaurant).ToListAsync();
            return Page();
        }
    }

    }
