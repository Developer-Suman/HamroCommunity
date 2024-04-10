using System.Net;

namespace HamroCommunity.CustomMiddleware.CustomException
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
            
        }
        public NotFoundException(string message) : base(message)
        {
            
        }

        //Define the property for the status code
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound; //NotFound
    }
}
