using AI_Content_Assistant.Data;
using AI_Content_Assistant.DTOs;
using AI_Content_Assistant.Exceptions;
using AI_Content_Assistant.Models;
using Microsoft.EntityFrameworkCore;


namespace AI_Content_Assistant.Services
{
    public class AiContentService : IAiContentService
    {
        private readonly AppDbContext _db;

        public AiContentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AiContent> CreateAsync(AiContentRequestDto request, string generatedText)
        {
            var content = new AiContent
            {
                Prompt = request.Prompt,
                GeneratedText = generatedText,
                CreatedAt = DateTime.UtcNow
            };

            _db.AiContents.Add(content);
            await _db.SaveChangesAsync();

            return content;
        }


        public async Task<AiContent> GetByIdAsync(int id)
        {
            var content = await _db.AiContents.FindAsync(id);

            if (content == null)
                throw new ContentNotFoundException($"Content with ID {id} not found.");

            return content;
        }


        public async Task<List<AiContent>> GetAllByDateAsync(DateTime? date)
        {
            var query = _db.AiContents.AsQueryable();

            if (date.HasValue)
            {
                var day = date.Value.Date;
                query = query.Where(c => c.CreatedAt.Date == day);
            }

            return await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }



        public async Task<AiContent> UpdateAsync(int id, AiContentUpdateDto request, string generatedText)
        {
            var content = await _db.AiContents.FindAsync(id);

            if (content == null)
                throw new ContentNotFoundException($"Content with ID {id} not found.");

            content.Prompt = request.Prompt;
            content.GeneratedText = generatedText;

            await _db.SaveChangesAsync();

            return content;
        }



        public async Task DeleteAsync(int id)
        {
            var content = await _db.AiContents.FindAsync(id);

            if (content == null)
                throw new ContentNotFoundException($"Content with ID {id} not found.");

            _db.AiContents.Remove(content);
            await _db.SaveChangesAsync();
        }

    }
}
