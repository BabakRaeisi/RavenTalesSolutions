using CoreLayer.ServiceContracts;
using RavenTales.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
 
 
    public sealed class DummyProfileClient : IProfileClient
    {
    
            public Task<UserProfileDto> GetAsync(Guid userId, CancellationToken ct = default) =>
                Task.FromResult(new UserProfileDto(userId, LanguageLevel.B1, Language.Spanish, Language.English));
       
    }
 
