using System.Net;

namespace HamroCommunity.CustomMiddleware.CustomException
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException()
        {
            
        }

        public UnAuthorizedException(string message) : base(message)
        {
            
        }


        //Define Property for status code
        public HttpStatusCode StatusCode { get { return HttpStatusCode.Unauthorized; } }
    }
}
