
using MinorProject3.Data.Models;
using Microsoft.Data.SqlClient;

namespace MinorProject3.Data.Repositories
{
    public class GameRepository
    {
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS01;Database=MinorProject3Db;Trusted_Connection=True;TrustServerCertificate=True;";

        // --- QUESTS CRUD & LISTING ---
        public List<Quest> GetAllQuests()
        {
            var quests = new List<Quest>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Title, Difficulty, RewardXp, IsCompleted FROM Quests", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                quests.Add(new Quest
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Difficulty = reader.GetString(2),
                    RewardXp = reader.GetInt32(3),
                    IsCompleted = reader.GetBoolean(4)
                });
            }
            return quests;
        }

        public Quest? GetQuestById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Title, Difficulty, RewardXp, IsCompleted FROM Quests WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Quest
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Difficulty = reader.GetString(2),
                    RewardXp = reader.GetInt32(3),
                    IsCompleted = reader.GetBoolean(4)
                };
            }
            return null;
        }

        public void AddQuest(Quest quest)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO Quests (Title, Difficulty, RewardXp, IsCompleted) VALUES (@Title, @Difficulty, @RewardXp, @IsCompleted)", conn);
            cmd.Parameters.AddWithValue("@Title", quest.Title);
            cmd.Parameters.AddWithValue("@Difficulty", quest.Difficulty);
            cmd.Parameters.AddWithValue("@RewardXp", quest.RewardXp);
            cmd.Parameters.AddWithValue("@IsCompleted", quest.IsCompleted);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateQuest(Quest quest)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE Quests SET Title = @Title, Difficulty = @Difficulty, RewardXp = @RewardXp, IsCompleted = @IsCompleted WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", quest.Id);
            cmd.Parameters.AddWithValue("@Title", quest.Title);
            cmd.Parameters.AddWithValue("@Difficulty", quest.Difficulty);
            cmd.Parameters.AddWithValue("@RewardXp", quest.RewardXp);
            cmd.Parameters.AddWithValue("@IsCompleted", quest.IsCompleted);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteQuest(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Quests WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- ITEMS & MULTI-FIELD SEARCH ---
        public List<Item> SearchItems(string searchTerm, string itemTypeFilter)
        {
            var items = new List<Item>();
            using var conn = new SqlConnection(_connectionString);

            string query = "SELECT Id, ItemName, ItemType, Rarity, ValueGold FROM Items WHERE 1=1";

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " AND (ItemName LIKE @Search OR Rarity LIKE @Search)";
            }
            if (!string.IsNullOrEmpty(itemTypeFilter))
            {
                query += " AND ItemType = @ItemType";
            }

            using var cmd = new SqlCommand(query, conn);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                cmd.Parameters.AddWithValue("@Search", $"%{searchTerm}%");
            }
            if (!string.IsNullOrEmpty(itemTypeFilter))
            {
                cmd.Parameters.AddWithValue("@ItemType", itemTypeFilter);
            }

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    ItemType = reader.GetString(2),
                    Rarity = reader.GetString(3),
                    ValueGold = reader.GetInt32(4)
                });
            }
            return items;
        }

        public Item? GetItemById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, ItemName, ItemType, Rarity, ValueGold FROM Items WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Item
                {
                    Id = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    ItemType = reader.GetString(2),
                    Rarity = reader.GetString(3),
                    ValueGold = reader.GetInt32(4)
                };
            }
            return null;
        }

        public void AddItem(Item item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Items (ItemName, ItemType, Rarity, ValueGold) VALUES (@ItemName, @ItemType, @Rarity, @ValueGold)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                cmd.Parameters.AddWithValue("@ItemType", item.ItemType);
                cmd.Parameters.AddWithValue("@Rarity", item.Rarity ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ValueGold", item.ValueGold);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateItem(Item item)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE Items SET ItemName = @ItemName, ItemType = @ItemType, Rarity = @Rarity, ValueGold = @ValueGold WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", item.Id);
            cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
            cmd.Parameters.AddWithValue("@ItemType", item.ItemType);
            cmd.Parameters.AddWithValue("@Rarity", item.Rarity ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ValueGold", item.ValueGold);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteItem(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Items WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // --- GUILD SHOP ---
        public List<ShopItem> GetAllShopItems()
        {
            var shopItems = new List<ShopItem>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, ItemName, ItemType, PriceGold, StockQuantity FROM ShopItems WHERE StockQuantity > 0", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                shopItems.Add(new ShopItem
                {
                    Id = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    ItemType = reader.GetString(2),
                    PriceGold = reader.GetInt32(3),
                    StockQuantity = reader.GetInt32(4)
                });
            }
            return shopItems;
        }

        public ShopItem? GetShopItemById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, ItemName, ItemType, PriceGold, StockQuantity FROM ShopItems WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ShopItem
                {
                    Id = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    ItemType = reader.GetString(2),
                    PriceGold = reader.GetInt32(3),
                    StockQuantity = reader.GetInt32(4)
                };
            }
            return null;
        }

        public void ProcessPurchase(ShopItem shopItem)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Decrement shop stock quantity
                using (var cmdStock = new SqlCommand("UPDATE ShopItems SET StockQuantity = StockQuantity - 1 WHERE Id = @Id AND StockQuantity > 0", conn, transaction))
                {
                    cmdStock.Parameters.AddWithValue("@Id", shopItem.Id);
                    cmdStock.ExecuteNonQuery();
                }

                // 2. Add bought item to main Items table (personal inventory)
                using (var cmdInventory = new SqlCommand("INSERT INTO Items (ItemName, ItemType, Rarity, ValueGold) VALUES (@ItemName, @ItemType, 'Purchased', @ValueGold)", conn, transaction))
                {
                    cmdInventory.Parameters.AddWithValue("@ItemName", shopItem.ItemName);
                    cmdInventory.Parameters.AddWithValue("@ItemType", shopItem.ItemType);
                    cmdInventory.Parameters.AddWithValue("@ValueGold", shopItem.PriceGold);
                    cmdInventory.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // --- ADMIN SETTINGS ---
        public List<AdminSetting> GetAdminSettings()
        {
            var settings = new List<AdminSetting>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, SettingKey, SettingValue, LastUpdated FROM AdminSettings", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                settings.Add(new AdminSetting
                {
                    Id = reader.GetInt32(0),
                    SettingKey = reader.GetString(1),
                    SettingValue = reader.GetString(2),
                    LastUpdated = reader.GetDateTime(3)
                });
            }
            return settings;
        }

        public AdminSetting? GetAdminSettingById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, SettingKey, SettingValue, LastUpdated FROM AdminSettings WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new AdminSetting
                {
                    Id = reader.GetInt32(0),
                    SettingKey = reader.GetString(1),
                    SettingValue = reader.GetString(2),
                    LastUpdated = reader.GetDateTime(3)
                };
            }
            return null;
        }

        public void AddAdminSetting(AdminSetting setting)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO AdminSettings (SettingKey, SettingValue, LastUpdated) VALUES (@SettingKey, @SettingValue, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SettingKey", setting.SettingKey);
                cmd.Parameters.AddWithValue("@SettingValue", setting.SettingValue);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateAdminSetting(AdminSetting setting)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE AdminSettings SET SettingKey = @SettingKey, SettingValue = @SettingValue, LastUpdated = GETDATE() WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", setting.Id);
            cmd.Parameters.AddWithValue("@SettingKey", setting.SettingKey);
            cmd.Parameters.AddWithValue("@SettingValue", setting.SettingValue);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAdminSetting(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM AdminSettings WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}