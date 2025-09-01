using CoreLayer.Entities;
using CoreLayer.RepositoryContracts;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly StoryDbContext _context;

        public StoryRepository(StoryDbContext context)
        {
            _context = context;
        }

        public async Task<Story> AddAsync(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }

        public async Task<Story?> GetByIdAsync(Guid id)
        {
            return await _context.Stories.FindAsync(id);
        }

        public async Task<IEnumerable<Story>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Stories
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story == null)
                return false;

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}