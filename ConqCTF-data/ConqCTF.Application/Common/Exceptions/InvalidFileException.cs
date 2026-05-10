namespace ConqCTF.Application.Common.Exceptions
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException(string message) : base(message) { }
    }
}