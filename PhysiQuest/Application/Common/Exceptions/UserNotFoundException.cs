﻿namespace Application.Common.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base($"User with ID {userId} not found.")
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }
        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
