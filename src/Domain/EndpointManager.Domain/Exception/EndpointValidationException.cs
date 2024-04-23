namespace EndpointManager.Domain.Exceptions
{
    public class EndpointValidationException : Exception
    {
        public EndpointValidationException(string? message) : base(message) { }
    }
}
