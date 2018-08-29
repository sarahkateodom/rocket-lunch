using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace makeLunch.domain.utilities
{
    /// <summary>
    /// Dto used to transfer info from Domain to Controller
    /// </summary>
    public class HttpStatusCodeErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeErrorResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseMessage">The response message.</param>
        public HttpStatusCodeErrorResponse(HttpStatusCode statusCode, string responseMessage)
        {
            HttpErrorStatusCode = statusCode;
            ErrorResponseMessage = responseMessage;
        }

        /// <summary>
        /// Gets the HTTP error status code.
        /// </summary>
        /// <value>
        /// The HTTP error status code.
        /// </value>
        public HttpStatusCode HttpErrorStatusCode { get; }

        /// <summary>
        /// Gets the error response message.
        /// </summary>
        /// <value>
        /// The error response message.
        /// </value>
        public string ErrorResponseMessage { get; }

        /// <summary>
        /// Basically the same as calling Content(HttpErrorStatusCode, ErrorResponseMessage)
        /// from within the controller. E.g. (in controller): return err.Content(this)
        /// </summary>
        /// <value>
        /// NegotiatedContentResult
        /// </value>
        public ObjectResult Content(Controller apiController)
        {
            return new ObjectResult(ErrorResponseMessage) { StatusCode = (int)HttpErrorStatusCode };
        }
    }
}