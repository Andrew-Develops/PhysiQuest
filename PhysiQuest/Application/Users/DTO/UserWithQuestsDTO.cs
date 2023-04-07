using Application.UserQuests.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTO
{
    public class UserWithQuestsDTO : UserDTO
    {
        public List<UserQuestDTO> UserQuests { get; set; }
    }

}
