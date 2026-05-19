using LLM_Proxy_API.Clients;
using LLM_Proxy_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LLM_Proxy_API.Controllers
{
    [ApiController]
    [Route("api/llm")]
    public class LlmController : ControllerBase
    {
        private readonly OllamaClient _ollamaClient;

        public LlmController(OllamaClient ollamaClient)
        {
            _ollamaClient = ollamaClient;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<LlmResponseDto>> GenerateAsync(
            [FromBody] LlmRequestDto request,
            CancellationToken ct)
        {
            // 1. Call Ollama through the client
            var generatedText = await _ollamaClient.GenerateAsync(request.Prompt, ct);

            // 2. Wrap the result in a DTO for Service A
            var response = new LlmResponseDto(generatedText);

            // 3. Return the DTO
            return Ok(response);
        }
    }
}
