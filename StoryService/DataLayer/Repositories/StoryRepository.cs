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

        public async Task<Story?> GetStoryById(Guid id)
        {
            return await _context.Stories
                .Include(s => s.Sentences)
                .Include(s => s.Units) // owned Segments come along with Units
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Story>> FindStoriesByFilterAsync(
            LanguageLevel languageLevel, TargetLanguage targetLanguage, string? topic = null)
        {
            var query = _context.Stories
                .Include(s => s.Sentences)
                .Include(s => s.Units)
                .AsNoTracking()
                .Where(s => s.LanguageLevel == languageLevel && s.TargetLanguage == targetLanguage);

            if (!string.IsNullOrEmpty(topic))
                query = query.Where(s => s.Title.Contains(topic) || s.Content.Contains(topic) || s.Genre.Contains(topic));

            return await query
                .OrderByDescending(s => s.CreatedAt)   // prefer freshest first
                .ToListAsync();
        }




    }
}