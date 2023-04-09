namespace Application.Common.Exceptions
{
    public class DuplicateBadgeException : Exception
    {
        public DuplicateBadgeException(string message) : base(message)
        {
        }
        public DuplicateBadgeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
