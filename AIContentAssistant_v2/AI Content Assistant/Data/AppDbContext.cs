using AI_Content_Assistant.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_Content_Assistant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AiContent> AiContents => Set<AiContent>();
    }
}
