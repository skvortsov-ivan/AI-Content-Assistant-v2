namespace LLM_Proxy_API.DTOs
{
    public record OllamaRequestDto(
        string Model,
        string Prompt,
        bool Stream
    );
    
    
    public record OllamaResponseDto
    {
        public string response { get; set; } = "";
    }
}
