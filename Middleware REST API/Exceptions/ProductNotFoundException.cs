namespace Middleware_REST_API.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() 
        {

        }

        public ProductNotFoundException(string message)
        : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
