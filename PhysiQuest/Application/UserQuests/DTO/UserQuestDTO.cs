using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserQuests.DTO
{
    public class UserQuestDTO
    {
        public int UserId { get; set; }
        public int QuestId { get; set; }
        public byte[] ProofImage { get; set; }
        public string Status { get; set; }
    }

}
