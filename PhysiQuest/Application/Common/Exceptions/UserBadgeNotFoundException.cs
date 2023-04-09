namespace Application.Common.Exceptions
{
    public class UserBadgeNotFoundException : Exception
    {
        public UserBadgeNotFoundException(int userBadgeId)
            : base($"User badge with ID {userBadgeId} was not found.")
        {
        }
        public UserBadgeNotFoundException(string message) : base(message)
        {
        }

        public UserBadgeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
