using CoreLayer.Entities;
using CoreLayer.Enums;
using CoreLayer.RepositoryContracts;
using DataLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Story>> FindStoriesByFilterAsync(
            LanguageLevel languageLevel,
            TargetLanguage targetLanguage,
            string? topic = null)
        {
            var query = _context.Stories
                .Include(s => s.Sentences)
                .Include(s => s.Units)
                .Where(s => s.LanguageLevel == languageLevel &&
                           s.TargetLanguage == targetLanguage);

            if (!string.IsNullOrEmpty(topic))
            {
                // Search for topic in title, content, or genre
                query = query.Where(s =>
                    s.Title.Contains(topic) ||
                    s.Content.Contains(topic) ||
                    s.Genre.Contains(topic));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Story>> GetStoriesByIdsAsync(IEnumerable<Guid> storyIds)
        {
            // If empty list, return empty result to avoid unnecessary query
            if (!storyIds.Any())
            {
                return Enumerable.Empty<Story>();
            }

            return await _context.Stories
                .Include(s => s.Sentences)
                .Include(s => s.Units)
                .Where(s => storyIds.Contains(s.Id))
                .ToListAsync();
        }

      
    }
}