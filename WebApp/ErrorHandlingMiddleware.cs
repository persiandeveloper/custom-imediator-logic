
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationLogic
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ObjectResult errorResponse;

            // If appropriate your can access your security token
            // from the HttpContext here to enrich the error response with the clients correlation id.
            try
            {
                await _next(context);
                return;
            }
            catch (Exception exception)
            {
                var a = 20;
            }

            await context.Response.WriteAsync("hello");
        }
    }
}