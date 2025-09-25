using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Entities;
using CoreLayer.RepositoryContracts;

namespace DataLayer.Repositories;


public class UserHistoryRepository : IUserStoryHistoryRepository
{
    public Task<Story> Insert(Guid userId, Guid storyId, DateTime dateUtc, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<Story>> List(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Task ClearHistory(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

}

