using CoreLayer.ServiceContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services
{
    public sealed class InMemoryUserSkipStore : IUserSkipStore
    {
        private readonly ConcurrentDictionary<Guid, HashSet<Guid>> _skips = new();
        public HashSet<Guid> GetSkipped(Guid userId) =>
            _skips.GetOrAdd(userId, _ => new HashSet<Guid>());
        public void AddSkip(Guid userId, Guid storyId) =>
            GetSkipped(userId).Add(storyId);
    }
}
