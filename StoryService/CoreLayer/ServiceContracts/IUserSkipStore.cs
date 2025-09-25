using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ServiceContracts
{
    public  interface IUserSkipStore
    {
        HashSet<Guid> GetSkipped(Guid userId);
        void AddSkip(Guid userId, Guid storyId);
    }
}
