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
    public class DeleteModel : PageModel
    {
        private readonly CafeteriaOrderingApp.Services.ApplicationDbContext _context;

        public DeleteModel(CafeteriaOrderingApp.Services.ApplicationDbContext context)
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

            var menuitem = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);

            if (menuitem is not null)
            {
                MenuItem = menuitem;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuitem = await _context.MenuItems.FindAsync(id);
            if (menuitem != null)
            {
                MenuItem = menuitem;
                _context.MenuItems.Remove(MenuItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
