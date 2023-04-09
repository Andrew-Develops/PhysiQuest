using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class UserNameNotFoundException : Exception
    {
        public UserNameNotFoundException(string userName)
            : base($"User with username {userName} not found.")
        {
        }
    }

}
