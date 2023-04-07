using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class BadgeDeletionFailedException : Exception
    {
        public BadgeDeletionFailedException(int id) : base($"Failed to delete badge with ID {id}.")
        {
        }
    }
}
