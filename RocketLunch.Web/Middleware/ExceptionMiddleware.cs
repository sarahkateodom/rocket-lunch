using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RocketLunch.domain.exceptions;

namespace RocketLunch.web.middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // dynamically set HttpStatusCode based on exception type 
            switch(ex) {
                case BadRequestException ex1 :
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException ex2 :
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case TooManyRequestsException ex2 :
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    break;                
            }

            return context.Response.WriteAsync(ex.Message);
        }
    }
}