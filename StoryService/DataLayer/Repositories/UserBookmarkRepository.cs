using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.RepositoryContracts;
namespace DataLayer.Repositories
{
    public class UserBookmarkRepository : IUserStoryBookmarkRepository
    {
        public void Add(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> List(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid userId, Guid storyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
