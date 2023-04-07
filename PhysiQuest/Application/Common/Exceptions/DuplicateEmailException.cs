using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string email)
            : base($"An account with email '{email}' already exists.")
        {
        }
    }
}
