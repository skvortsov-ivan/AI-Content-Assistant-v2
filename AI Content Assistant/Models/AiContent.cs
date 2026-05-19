namespace AI_Content_Assistant.Models
{
    public class AiContent
    {
        public int Id { get; set; }
        public string Prompt { get; set; } = "";
        public string GeneratedText { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }


}
