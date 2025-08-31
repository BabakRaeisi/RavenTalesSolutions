using CoreLayer.Entities;

namespace CoreLayer.RepositoryContracts
{
    public interface IStoryRepository
    {
        Task<Story> AddAsync(Story story);
        Task<Story?> GetByIdAsync(Guid id);
        Task<IEnumerable<Story>> GetByUserIdAsync(Guid userId);
        Task<bool> DeleteAsync(Guid id);

    }
}