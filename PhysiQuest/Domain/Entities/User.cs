using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int Tokens { get; set; }
        public List<UserQuest> UserQuests { get; set; }
        public List<UserBadge> UserBadges { get; set; }
    }
}
