namespace Misc
{
    public class LogInfo
    {
        public string Message { get; private set; }

        public LogInfo(string message)
        {
            Message = message;
        }
    }
}
