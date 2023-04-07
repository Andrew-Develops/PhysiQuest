using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class QuestDeletionFailedException : Exception
    {
        public QuestDeletionFailedException(int questId)
            : base($"Deletion of quest with id {questId} failed.")
        {
        }
    }

}
