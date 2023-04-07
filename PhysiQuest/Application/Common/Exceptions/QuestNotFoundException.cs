using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class QuestNotFoundException : Exception
    {
        public QuestNotFoundException(int questId)
            : base($"Quest with ID {questId} was not found.")
        {
        }
    }
}
