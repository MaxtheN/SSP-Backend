namespace SspUis.RabbitMQ.Doc.Exceptions
{
    public class InvalidStatusChangedException : Exception
    {
        public InvalidStatusChangedException()
        {
        }

        public InvalidStatusChangedException(string message)
            : base(message)
        {
        }

        public InvalidStatusChangedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
