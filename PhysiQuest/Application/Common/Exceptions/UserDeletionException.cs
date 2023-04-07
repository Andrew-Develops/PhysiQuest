using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class UserDeletionException : Exception
    {
        public UserDeletionException(string message) : base(message)
        {
        }

        public UserDeletionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
