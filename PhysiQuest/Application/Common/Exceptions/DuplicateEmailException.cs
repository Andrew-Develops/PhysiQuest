namespace Application.Common.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string email)
            : base($"An account with email '{email}' already exists.")
        {
        }
        public DuplicateEmailException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
