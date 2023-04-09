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
