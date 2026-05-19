using LLM_Proxy_API.Middlewares;

namespace LLM_Proxy_API.Extensions
{
    public static class ApiKeyExtensions
    {
        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
