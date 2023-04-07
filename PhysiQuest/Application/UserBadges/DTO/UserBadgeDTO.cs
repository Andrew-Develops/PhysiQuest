using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserBadges.DTO
{
    public class UserBadgeDTO
    {
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime AwardDate { get; set; }
    }

}
