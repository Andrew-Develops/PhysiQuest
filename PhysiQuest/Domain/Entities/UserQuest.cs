namespace Domain.Entities
{
    public class UserQuest
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestId { get; set; }
        public Quest Quest { get; set; }
        public byte[] ProofImage { get; set; }
        public string Status { get; set; }
    }
}
