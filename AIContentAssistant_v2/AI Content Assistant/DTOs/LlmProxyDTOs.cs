namespace AI_Content_Assistant.DTOs
{
    public record LlmRequestDto(string Prompt);

    public record LlmResponseDto(string Answer);

}
