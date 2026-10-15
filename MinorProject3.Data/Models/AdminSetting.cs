namespace MinorProject3.Data.Models
{
    public class AdminSetting
    {
        public int Id { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}