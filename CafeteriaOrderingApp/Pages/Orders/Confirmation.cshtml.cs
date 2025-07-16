using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CafeteriaOrderingApp.Pages.Orders
{
    public class ConfirmationModel : PageModel
    {
        public int Id { get; set; }

        public void OnGet(int id)
        {
            Id = id;
        }
    }
}
