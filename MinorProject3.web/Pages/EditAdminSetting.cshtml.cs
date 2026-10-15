using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class EditAdminSettingModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        [BindProperty]
        public AdminSetting Setting { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var setting = _repository.GetAdminSettingById(id);
            if (setting == null)
            {
                return RedirectToPage("/AdminSettings");
            }

            Setting = setting;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.UpdateAdminSetting(Setting);
            TempData["SuccessMessage"] = "Admin setting updated successfully!";
            return RedirectToPage("/AdminSettings");
        }
    }
}