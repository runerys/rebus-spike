namespace SagaDemo.Messages
{
    public class LogMePlease
    {
        public LogMePlease(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}