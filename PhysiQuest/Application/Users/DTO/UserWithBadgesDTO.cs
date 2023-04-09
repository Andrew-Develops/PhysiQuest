using Application.UserBadges.DTO;

namespace Application.Users.DTO
{
    public class UserWithBadgesDTO : UserDTO
    {
        public List<UserBadgeDTO> UserBadges { get; set; }
    }

}
