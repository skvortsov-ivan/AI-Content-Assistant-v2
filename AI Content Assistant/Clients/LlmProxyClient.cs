using System.Net.Http.Json;
using AI_Content_Assistant.DTOs;

namespace AI_Content_Assistant.Clients
{
    public class LlmProxyClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public LlmProxyClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<LlmResponseDto> GenerateAsync(string prompt, CancellationToken ct)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/llm/generate")
            {
                Content = JsonContent.Create(new { prompt })
            };

            // Add API key header
            var apiKey = _config["ServiceB:ApiKey"];
            request.Headers.Add("X-API-KEY", apiKey);

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LlmResponseDto>(cancellationToken: ct);
        }
    }
}
