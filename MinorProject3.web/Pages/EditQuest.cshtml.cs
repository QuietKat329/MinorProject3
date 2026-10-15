using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class EditQuestModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        [BindProperty]
        public Quest Quest { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var quest = _repository.GetQuestById(id);
            if (quest == null)
            {
                return RedirectToPage("/Index");
            }

            Quest = quest;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _repository.UpdateQuest(Quest);
            TempData["SuccessMessage"] = "Quest updated successfully!";
            return RedirectToPage("/Index");
        }
    }
}