namespace Application.Common.Exceptions
{
    public class InsufficientTokensException : Exception
    {
        public InsufficientTokensException()
        {
        }

        public InsufficientTokensException(string message)
            : base(message)
        {
        }

        public InsufficientTokensException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
