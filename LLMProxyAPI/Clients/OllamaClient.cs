using System.Net.Http.Json;
using LLM_Proxy_API.DTOs;

namespace LLM_Proxy_API.Clients
{
    public class OllamaClient
    {
        private readonly HttpClient _http;

        public OllamaClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            // Build DTO with hardcoded model + stream
            var requestDto = new OllamaRequestDto(
                Model: "llama3.2",
                Prompt: prompt,
                Stream: false
            );

            // Send request to Ollama
            var httpResponse = await _http.PostAsJsonAsync(
                "api/generate",
                requestDto,
                cancellationToken: ct
            );

            httpResponse.EnsureSuccessStatusCode();

            // Deserialize response
            var responseDto = await httpResponse.Content.ReadFromJsonAsync<OllamaResponseDto>(
                cancellationToken: ct
            );

            return responseDto?.response ?? "";
        }
    }
}
