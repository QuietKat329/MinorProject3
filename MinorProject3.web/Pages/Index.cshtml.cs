using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        public List<Quest> Quests { get; set; } = new();

        [BindProperty]
        public Quest NewQuest { get; set; } = new();

        // Player Stats Properties (Loaded from Cookies)
        public int PlayerGold { get; set; } = 500;
        public int PlayerXp { get; set; } = 100;
        public int PlayerLevel => 1 + (PlayerXp / 500); // 1 Level per 500 XP

        public void OnGet()
        {
            LoadPlayerStats();
            Quests = _repository.GetAllQuests();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadPlayerStats();
                Quests = _repository.GetAllQuests();
                return Page();
            }

            _repository.AddQuest(NewQuest);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _repository.DeleteQuest(id);
            return RedirectToPage();
        }

        public IActionResult OnPostCompleteQuest(int id)
        {
            var quest = _repository.GetQuestById(id);
            if (quest != null && !quest.IsCompleted)
            {
                quest.IsCompleted = true;
                _repository.UpdateQuest(quest);

                LoadPlayerStats();
                PlayerGold += quest.RewardXp;
                PlayerXp += quest.RewardXp;
                SavePlayerStats();

                TempData["SuccessMessage"] = $" Quest Completed! You earned {quest.RewardXp} Gold & {quest.RewardXp} XP!";
            }

            return RedirectToPage();
        }

        private void LoadPlayerStats()
        {
            if (Request.Cookies.TryGetValue("PlayerGold", out var goldStr) && int.TryParse(goldStr, out int gold))
            {
                PlayerGold = gold;
            }
            else
            {
                SaveCookie("PlayerGold", PlayerGold.ToString());
            }

            if (Request.Cookies.TryGetValue("PlayerXp", out var xpStr) && int.TryParse(xpStr, out int xp))
            {
                PlayerXp = xp;
            }
            else
            {
                SaveCookie("PlayerXp", PlayerXp.ToString());
            }
        }

        private void SavePlayerStats()
        {
            SaveCookie("PlayerGold", PlayerGold.ToString());
            SaveCookie("PlayerXp", PlayerXp.ToString());
        }

        private void SaveCookie(string key, string value)
        {
            var options = new CookieOptions { Expires = DateTime.Now.AddYears(1), IsEssential = true };
            Response.Cookies.Append(key, value, options);
        }
    }
}