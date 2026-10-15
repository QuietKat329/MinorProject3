using MinorProject3.Data.Repositories;

namespace MinorProject3.Web.Helpers
{
    public static class LocalizationHelper
    {
        public static string GetString(string key, string lang = "en")
        {
            // You can expand this dictionary or load from resource files/database
            if (lang == "fr")
            {
                return key switch
                {
                    "QuestBoard" => "Tableau des Quêtes",
                    "GuildShop" => "Boutique de la Guilde",
                    "Gold" => "Or",
                    "Xp" => "EXP",
                    "ClaimReward" => "Réclamer la Récompense",
                    _ => key
                };
            }

            // Default English
            return key switch
            {
                "QuestBoard" => "Quest Board",
                "GuildShop" => "Guild Shop",
                "Gold" => "Gold",
                "Xp" => "XP",
                "ClaimReward" => "Claim Reward",
                _ => key
            };
        }
    }
}