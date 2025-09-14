using RavenTales.Core.DTO;

namespace RavenTales.Core.ServiceContracts; 
 
    public interface IUserService
    {
    /// <summary>
    /// method to login a user and return an authentication response
    /// </summary>
    /// <param name="LoginRequest"></param>
    /// <returns></returns>
    Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
    /// <summary>
    /// // method to register a user and return an authentication response that represents the user status
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
    }
 
