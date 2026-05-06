namespace ConqCTF.Application.Common.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException()
             : base("Too many requests. Please try again later.") { }
    }
}
