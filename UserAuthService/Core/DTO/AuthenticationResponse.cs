
using System.ComponentModel;

namespace RavenTales.Core.DTO;
 
    public record AuthenticationResponse(
        Guid UserID,
        string? Email,
        string? Password,
        string? PersonName,
        string? Token,
        bool IsSuccessful)
        {
    //parameterless cosnstructor for automapping 
    public AuthenticationResponse() : this(Guid.Empty, null, null, null, null, false) { }
    }





