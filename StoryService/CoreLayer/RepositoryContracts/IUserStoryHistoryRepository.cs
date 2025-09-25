using CoreLayer.Entities;
using RavenTales.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.RepositoryContracts
{
    public interface IUserStoryHistoryRepository
    {
        Task<Story> Insert(Guid userId, Guid storyId, DateTime dateUtc, CancellationToken ct = default);

       Task<IEnumerable<Story>> List(Guid userId, CancellationToken ct = default);

        Task ClearHistory(Guid userId, CancellationToken ct = default);
    }
 
}
