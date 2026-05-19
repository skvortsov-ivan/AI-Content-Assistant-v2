using AI_Content_Assistant.Clients;
using AI_Content_Assistant.DTOs;
using AI_Content_Assistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AI_Content_Assistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AiContentController : ControllerBase
    {
        private readonly LlmProxyClient _llmClient;
        private readonly IAiContentService _contentService;

        public AiContentController(LlmProxyClient llmClient, IAiContentService contentService)
        {
            _llmClient = llmClient;
            _contentService = contentService;
        }

        /// <summary>
        /// Generates new AI content using the Ollama LLM in Service B and stores it.
        /// </summary>
        /// <param name="request">The prompt used to generate new AI content.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The newly created AI content item.</returns>
        /// <response code="200">Content was generated and stored successfully.</response>
        /// <response code="400">The request body was invalid.</response>
        /// <response code="502">Service B or the LLM backend was unavailable.</response>
        [HttpPost("generate")]
        public async Task<ActionResult<AiContentResponseDto>> GenerateAsync(
            [FromBody] AiContentRequestDto request,
            CancellationToken ct)
        {
            // 1. Ask Service B to generate text
            var llmResponse = await _llmClient.GenerateAsync(request.Prompt, ct);

            // 2. Store the generated content in memory
            var saved = await _contentService.CreateAsync(request, llmResponse.Answer);

            // 3. Build the response DTO
            var dto = new AiContentResponseDto
            {
                Id = saved.Id,
                Prompt = saved.Prompt,
                GeneratedText = saved.GeneratedText,
                CreatedAt = saved.CreatedAt
            };

            return Ok(dto);
        }

        /// <summary>
        /// Retrieves all AI content items, optionally filtered by creation date.
        /// </summary>
        /// <param name="date">Optional filter. Only items created on this date will be returned.</param>
        /// <returns>A list of AI content items.</returns>
        /// <response code="200">Content items were retrieved successfully.</response>
        [HttpGet]
        public async Task<ActionResult<List<AiContentResponseDto>>> GetAllAsync(
            [FromQuery] DateTime? date)
        {
            var items = await _contentService.GetAllByDateAsync(date);

            var dto = items.Select(c => new AiContentResponseDto
            {
                Id = c.Id,
                Prompt = c.Prompt,
                GeneratedText = c.GeneratedText,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Ok(dto);
        }

        /// <summary>
        /// Retrieves a single AI content item by its ID.
        /// </summary>
        /// <param name="id">The ID of the content item to retrieve.</param>
        /// <returns>The requested AI content item.</returns>
        /// <response code="200">The content item was found and returned.</response>
        /// <response code="404">No content item with the specified ID exists.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AiContentResponseDto>> GetByIdAsync(int id)
        {
            var item = await _contentService.GetByIdAsync(id);

            var dto = new AiContentResponseDto
            {
                Id = item.Id,
                Prompt = item.Prompt,
                GeneratedText = item.GeneratedText,
                CreatedAt = item.CreatedAt
            };

            return Ok(dto);
        }

        /// <summary>
        /// Updates an existing AI content item and regenerates its text using Service B.
        /// </summary>
        /// <param name="id">The ID of the content item to update.</param>
        /// <param name="request">The updated prompt used to regenerate the content.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated AI content item.</returns>
        /// <response code="200">The content item was updated successfully.</response>
        /// <response code="400">The request body was invalid.</response>
        /// <response code="404">No content item with the specified ID exists.</response>
        /// <response code="502">Service B or the LLM backend was unavailable.</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AiContentResponseDto>> UpdateAsync(
            int id,
            [FromBody] AiContentUpdateDto request,
            CancellationToken ct)
        {
            var llmResponse = await _llmClient.GenerateAsync(request.Prompt, ct);
            var newText = llmResponse.Answer;

            var updated = await _contentService.UpdateAsync(id, request, newText);

            var dto = new AiContentResponseDto
            {
                Id = updated.Id,
                Prompt = updated.Prompt,
                GeneratedText = updated.GeneratedText,
                CreatedAt = updated.CreatedAt
            };

            return Ok(dto);
        }

        /// <summary>
        /// Deletes an AI content item by its ID.
        /// </summary>
        /// <param name="id">The ID of the content item to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">The content item was deleted successfully.</response>
        /// <response code="404">No content item with the specified ID exists.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _contentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
