namespace Application.Common.Exceptions
{
    public class BadgeDeletionFailedException : Exception
    {
        public BadgeDeletionFailedException(int id)
            : base($"Failed to delete badge with ID {id}.")
        {
        }
        public BadgeDeletionFailedException(string message) : base(message)
        {
        }

        public BadgeDeletionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
