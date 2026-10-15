using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class SearchItems : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        public List<Item> Items { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SearchId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ItemTypeFilter { get; set; }

        [BindProperty]
        public Item NewItem { get; set; } = new();

        public void OnGet()
        {
            var results = _repository.SearchItems(SearchTerm, ItemTypeFilter);

            if (SearchId.HasValue)
            {
                results = results.Where(i => i.Id == SearchId.Value).ToList();
            }

            Items = results;
        }

        public IActionResult OnPostDelete(int id)
        {
            _repository.DeleteItem(id);
            return RedirectToPage(new { SearchTerm = SearchTerm, ItemTypeFilter = ItemTypeFilter });
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                Items = _repository.SearchItems(SearchTerm, ItemTypeFilter);
                return Page();
            }

            _repository.AddItem(NewItem);
            return RedirectToPage(new { SearchTerm = SearchTerm, ItemTypeFilter = ItemTypeFilter });
        }
    }
}