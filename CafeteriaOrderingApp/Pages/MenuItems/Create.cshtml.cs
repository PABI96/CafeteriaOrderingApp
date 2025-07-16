using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;

namespace CafeteriaOrderingApp.Pages.MenuItems
{
    public class CreateModel : PageModel
    {
        private readonly CafeteriaOrderingApp.Services.ApplicationDbContext _context;

        public CreateModel(CafeteriaOrderingApp.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MenuItems.Add(MenuItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
