namespace SspUis.RabbitMQ.Doc.Exceptions
{
    public class CheckStatusdException : Exception
    {
        public CheckStatusdException()
        {
        }

        public CheckStatusdException(string message)
            : base(message)
        {
        }

        public CheckStatusdException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
