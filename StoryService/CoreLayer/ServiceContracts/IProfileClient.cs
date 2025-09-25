using RavenTales.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.ServiceContracts
{
    
        public interface IProfileClient
        {
            Task<UserProfileDto> GetAsync(Guid userId, CancellationToken ct = default);
        }
    
}
