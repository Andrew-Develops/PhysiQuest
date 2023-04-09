namespace Application.Common.Exceptions
{
    public class UserQuestNotCompletedException : Exception
    {
        public UserQuestNotCompletedException(string message) : base(message) { }
    }

}
