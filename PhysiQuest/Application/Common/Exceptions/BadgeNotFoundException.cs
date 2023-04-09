namespace Application.Common.Exceptions
{
    public class BadgeNotFoundException : Exception
    {
        public BadgeNotFoundException(int badgeId)
            : base($"Badge with ID {badgeId} was not found.")
        {
        }

        public BadgeNotFoundException(string message) : base(message)
        {
        }
        public BadgeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
