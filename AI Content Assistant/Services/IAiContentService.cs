using AI_Content_Assistant.DTOs;
using AI_Content_Assistant.Models;

namespace AI_Content_Assistant.Services
{
    public interface IAiContentService
    {
        Task<AiContent> CreateAsync(AiContentRequestDto request, string generatedText);
        Task<AiContent> GetByIdAsync(int id);
        Task<List<AiContent>> GetAllByDateAsync(DateTime? date);

        Task<AiContent> UpdateAsync(int id, AiContentUpdateDto request, string generatedText);

        Task DeleteAsync(int id);
    }
}

