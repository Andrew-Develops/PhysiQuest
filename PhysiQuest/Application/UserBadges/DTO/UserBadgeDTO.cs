namespace Application.UserBadges.DTO
{
    public class UserBadgeDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BadgeId { get; set; }
        public string BadgeName { get; set; }
        public DateTime AwardDate { get; set; }
    }

}
