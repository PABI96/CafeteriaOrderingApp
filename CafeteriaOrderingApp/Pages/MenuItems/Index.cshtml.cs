using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;

namespace CafeteriaOrderingApp.Pages.MenuItems
{
    public class IndexModel : PageModel
    {
        private readonly CafeteriaOrderingApp.Services.ApplicationDbContext _context;

        public IndexModel(CafeteriaOrderingApp.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> MenuItem { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public async Task<IActionResult> OnGetAsync(int restaurantId)
        {
            RestaurantId = restaurantId;

            var restaurant = await _context.Restaurants.FindAsync(RestaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            RestaurantName = restaurant.Name;


            MenuItem = await _context.MenuItems
            .Where(m => m.RestaurantId == RestaurantId)
            .ToListAsync();

            return Page();
        }
    }
}
