using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class DuplicateQuestException : Exception
    {
        public DuplicateQuestException(string message) : base(message)
        {
        }

        public DuplicateQuestException(string questName, string message) : base($"Quest with name {questName} already exists. {message}")
        {
        }
    }

}
