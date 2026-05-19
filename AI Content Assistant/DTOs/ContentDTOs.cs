namespace AI_Content_Assistant.DTOs
{
    /// <summary>
    /// Represents the request body for generating new AI content.
    /// </summary>
    public record AiContentRequestDto
    {
        /// <summary>
        /// The text prompt used to generate AI content.
        /// </summary>
        public string Prompt { get; init; } = string.Empty;
    }



    /// <summary>
    /// Represents the response returned for an AI content item.
    /// </summary>
    public record AiContentResponseDto
    {
        /// <summary>
        /// The unique identifier of the content item.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The original prompt used to generate the AI content.
        /// </summary>
        public string Prompt { get; init; } = string.Empty;

        /// <summary>
        /// The generated AI text returned from Service B.
        /// </summary>
        public string GeneratedText { get; init; } = string.Empty;

        /// <summary>
        /// The timestamp when the content item was created.
        /// </summary>
        public DateTime CreatedAt { get; init; }
    }



    /// <summary>
    /// Represents the request body for updating an existing AI content item.
    /// </summary>
    public record AiContentUpdateDto
    {
        /// <summary>
        /// The updated prompt used to regenerate the AI content.
        /// </summary>
        public string Prompt { get; init; } = string.Empty;
    }


}
