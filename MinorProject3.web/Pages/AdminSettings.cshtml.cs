using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class AdminSettingsModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        public List<AdminSetting> Settings { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? AdminCode { get; set; }

        public bool IsAuthorized { get; set; } = false;

        [BindProperty]
        public AdminSetting NewSetting { get; set; } = new();

        public IActionResult OnGet()
        {
            if (AdminCode == "DEV123")
            {
                IsAuthorized = true;
                Settings = _repository.GetAdminSettings();
            }
            else
            {
                IsAuthorized = false;
            }
            return Page();
        }

        public IActionResult OnPostAuthenticate()
        {
            if (AdminCode == "DEV123")
            {
                return RedirectToPage(new { AdminCode = AdminCode });
            }
            IsAuthorized = false;
            return Page();
        }

        public IActionResult OnPostAdd()
        {
            if (AdminCode == "DEV123")
            {
                _repository.AddAdminSetting(NewSetting);
            }
            return RedirectToPage(new { AdminCode = AdminCode });
        }

        public IActionResult OnPostDelete(int id)
        {
            if (AdminCode == "DEV123")
            {
                _repository.DeleteAdminSetting(id);
            }
            return RedirectToPage(new { AdminCode = AdminCode });
        }
    }
}