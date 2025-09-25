using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.RepositoryContracts
{
    public interface IUserStoryBookmarkRepository
    {
        void Add(Guid userId, Guid storyId, CancellationToken ct = default);
        void Remove(Guid userId, Guid storyId, CancellationToken ct = default);
        IEnumerable<Guid> List(Guid userId, CancellationToken ct = default);
    }
}
