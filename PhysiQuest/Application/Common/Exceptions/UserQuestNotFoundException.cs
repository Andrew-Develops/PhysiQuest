namespace Application.Common.Exceptions
{
    public class UserQuestNotFoundException : Exception
    {
        public UserQuestNotFoundException(int userQuestId) : base($"User quest with id {userQuestId} was not found.")
        {
        }

        public UserQuestNotFoundException(string message) : base(message)
        {
        }

        public UserQuestNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
