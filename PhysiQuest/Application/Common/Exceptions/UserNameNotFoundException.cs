namespace Application.Common.Exceptions
{
    public class UserNameNotFoundException : Exception
    {
        public UserNameNotFoundException(string userName)
            : base($"User with username {userName} not found.")
        {
        }

        public UserNameNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
