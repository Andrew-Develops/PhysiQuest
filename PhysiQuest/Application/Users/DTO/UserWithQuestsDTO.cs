using Application.UserQuests.DTO;

namespace Application.Users.DTO
{
    public class UserWithQuestsDTO : UserDTO
    {
        public List<UserQuestDTO> UserQuests { get; set; }
    }

}
