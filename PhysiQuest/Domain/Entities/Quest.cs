namespace Domain.Entities
{
    public class Quest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RewardPoints { get; set; }
        public int RewardTokens { get; set; }
        public List<UserQuest> UserQuests { get; set; }
    }
}
