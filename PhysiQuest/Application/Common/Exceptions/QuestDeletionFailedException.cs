namespace Application.Common.Exceptions
{
    public class QuestDeletionFailedException : Exception
    {
        public QuestDeletionFailedException(int questId)
            : base($"Deletion of quest with id {questId} failed.")
        {
        }
        public QuestDeletionFailedException(string message) : base(message)
        {
        }

        public QuestDeletionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
