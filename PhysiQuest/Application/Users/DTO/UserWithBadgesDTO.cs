using Application.UserBadges.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTO
{
    public class UserWithBadgesDTO : UserDTO
    {
        public List<UserBadgeDTO> UserBadges { get; set; }
    }

}
