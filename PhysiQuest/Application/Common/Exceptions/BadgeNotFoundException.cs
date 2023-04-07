using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class BadgeNotFoundException : Exception
    {
        public BadgeNotFoundException(int badgeId)
            : base($"Badge with ID {badgeId} was not found.")
        {
        }
    }
}
