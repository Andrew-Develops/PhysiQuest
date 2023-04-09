namespace Application.Quests.DTO
{
    public class CreateAndUpdateQuestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RewardPoints { get; set; }
        public int RewardTokens { get; set; }
    }
}
