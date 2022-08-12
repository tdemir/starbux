namespace WebApi.Helpers.Exceptions
{
    public class StarbuxValidationException : Exception
    {
        public StarbuxValidationException(string? message) : base(message)
        {
        }
    }
}