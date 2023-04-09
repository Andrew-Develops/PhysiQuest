namespace Domain.Entities
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserBadge> UserBadges { get; set; }
    }
}
