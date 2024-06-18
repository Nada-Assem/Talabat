using System.Net;
using System.Text.Json;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Middlewares
{
    // By Convension
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public readonly ILogger<ExceptionMiddleware> logger; // Use ILogger<ExceptionMiddleware>
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message); //Development
                // Log Exception in (Database I Files) // Production

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var option = new JsonSerializerOptions(){
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                };
                var json = JsonSerializer.Serialize(response , option);

                httpContext.Response.WriteAsync(json);
            }
        }
    }
}
