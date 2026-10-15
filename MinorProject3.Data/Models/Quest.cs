namespace MinorProject3.Data.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Difficulty { get; set; }
        public int RewardXp { get; set; }
        public bool IsCompleted { get; set; }
    }
}