namespace Application.Common.Exceptions
{
    public class QuestNotFoundException : Exception
    {
        public QuestNotFoundException(int questId)
            : base($"Quest with ID {questId} was not found.")
        {
        }
        public QuestNotFoundException(string message) : base(message)
        {
        }

        public QuestNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
