using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class EditItemModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        [BindProperty]
        public Item Item { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var item = _repository.GetItemById(id);
            if (item == null)
            {
                return RedirectToPage("/SearchItems");
            }

            Item = item;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.UpdateItem(Item);
            TempData["SuccessMessage"] = "Item updated successfully!";
            return RedirectToPage("/SearchItems");
        }
    }
}