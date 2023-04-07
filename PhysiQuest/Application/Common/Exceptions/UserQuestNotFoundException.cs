using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class UserQuestNotFoundException : Exception
    {
        public UserQuestNotFoundException(int userQuestId)
            : base($"User quest with ID {userQuestId} was not found.")
        {
        }
    }
}
