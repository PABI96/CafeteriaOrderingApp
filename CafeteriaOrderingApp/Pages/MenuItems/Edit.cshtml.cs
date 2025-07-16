using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;

namespace CafeteriaOrderingApp.Pages.MenuItems
{
    public class EditModel : PageModel
    {
        private readonly CafeteriaOrderingApp.Services.ApplicationDbContext _context;

        public EditModel(CafeteriaOrderingApp.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuitem =  await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (menuitem == null)
            {
                return NotFound();
            }
            MenuItem = menuitem;
           ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MenuItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemExists(MenuItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
