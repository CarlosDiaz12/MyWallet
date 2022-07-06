using System;
using System.Net;

namespace MyWallet.ErrorHandlers
{
    public class WebAPIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public WebAPIException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

        public WebAPIException(
            Exception exception,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(exception.Message)
        {
            StatusCode = statusCode;
        }
    }
}
