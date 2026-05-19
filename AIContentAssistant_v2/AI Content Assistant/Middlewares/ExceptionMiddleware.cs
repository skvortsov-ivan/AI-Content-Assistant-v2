namespace AI_Content_Assistant.Middleware
{
    using AI_Content_Assistant.Exceptions;
    using Microsoft.AspNetCore.Mvc;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                var problem = MapToProblemDetails(ex);

                context.Response.StatusCode = problem.Status ?? 500;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problem);
            }
        }

        private ProblemDetails MapToProblemDetails(Exception ex)
        {
            return ex switch
            {
                ContentNotFoundException nf => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = nf.Message
                },

                BadRequestException br => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = br.Message
                },

                HttpRequestException => new ProblemDetails
                {
                    Status = StatusCodes.Status502BadGateway,
                    Title = "Bad Gateway",
                    Detail = "The external LLM service is unavailable."
                },

                TaskCanceledException or OperationCanceledException => new ProblemDetails
                {
                    Status = StatusCodes.Status504GatewayTimeout,
                    Title = "Gateway Timeout",
                    Detail = "The external LLM service did not respond in time."
                },

                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                }
            };
        }
    }
}
