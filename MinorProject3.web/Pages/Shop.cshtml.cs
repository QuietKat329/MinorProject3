using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinorProject3.Data.Models;
using MinorProject3.Data.Repositories;

namespace MinorProject3.web.Pages
{
    public class ShopModel : PageModel
    {
        private readonly GameRepository _repository = new GameRepository();

        public List<ShopItem> ShopItems { get; set; } = new();
        public int PlayerGold { get; set; } = 500;

        public void OnGet()
        {
            LoadPlayerGold();
            ShopItems = _repository.GetAllShopItems();
        }

        public IActionResult OnPostBuy(int id)
        {
            LoadPlayerGold();
            var item = _repository.GetShopItemById(id);

            if (item == null || item.StockQuantity <= 0)
            {
                TempData["ErrorMessage"] = "That item is currently out of stock!";
                return RedirectToPage();
            }

            if (PlayerGold < item.PriceGold)
            {
                TempData["ErrorMessage"] = "You do not have enough Gold to purchase this item!";
                return RedirectToPage();
            }

            
            PlayerGold -= item.PriceGold;
            SaveCookie("PlayerGold", PlayerGold.ToString());

            
            _repository.ProcessPurchase(item);

            TempData["SuccessMessage"] = $" Successfully purchased '{item.ItemName}' for {item.PriceGold} Gold! It has been added to your inventory.";
            return RedirectToPage();
        }

        private void LoadPlayerGold()
        {
            if (Request.Cookies.TryGetValue("PlayerGold", out var goldStr) && int.TryParse(goldStr, out int gold))
            {
                PlayerGold = gold;
            }
            else
            {
                SaveCookie("PlayerGold", PlayerGold.ToString());
            }
        }

        private void SaveCookie(string key, string value)
        {
            var options = new CookieOptions { Expires = DateTime.Now.AddYears(1), IsEssential = true };
            Response.Cookies.Append(key, value, options);
        }
    }
}