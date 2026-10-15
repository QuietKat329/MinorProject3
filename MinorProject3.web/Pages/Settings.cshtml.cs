using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MinorProject3.web.Pages
{
    public class SettingsModel : PageModel
    {
        [BindProperty]
        public string SelectedTheme { get; set; } = "dark-fantasy";

        [BindProperty]
        public string SelectedLanguage { get; set; } = "en";

        public void OnGet()
        {
            // Retrieve saved preferences from browser cookies if they exist
            if (Request.Cookies.TryGetValue("GuildTheme", out var theme))
            {
                SelectedTheme = theme ?? "dark-fantasy";
            }

            if (Request.Cookies.TryGetValue("GuildLanguage", out var lang))
            {
                SelectedLanguage = lang ?? "en";
            }
        }

        public IActionResult OnPost()
        {
            // Store preferences in cookies for 1 year so they persist across visits
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                IsEssential = true
            };

            Response.Cookies.Append("GuildTheme", SelectedTheme, cookieOptions);
            Response.Cookies.Append("GuildLanguage", SelectedLanguage, cookieOptions);

            TempData["SuccessMessage"] = " Settings saved successfully!";
            return RedirectToPage();
        }
    }
}