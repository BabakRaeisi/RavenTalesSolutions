using RavenTales.Core.DTO;
using RavenTales.Core.Entities;
using RavenTales.Core.RepositoryContracts;
using RavenTales.Core.ServiceContracts;
using AutoMapper;
namespace RavenTales.Core.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService; // Add this
    public UserService(IUserRepository userRepository,IMapper mapper,ITokenService tokenService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
         ApplicationUser? user = await _userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);
        if (user == null) return null;

        string token = _tokenService.GenerateToken(user);

        return _mapper.Map<AuthenticationResponse>(user) with {IsSuccessful=true ,Token=token };
        

    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        ApplicationUser user = _mapper.Map<ApplicationUser>(registerRequest);
        ApplicationUser? registeredUser = await _userRepository.AddUser(user);

        if (registeredUser == null) return null;

        string token = _tokenService.GenerateToken(registeredUser);

        return _mapper.Map<AuthenticationResponse>(registeredUser) with
        {
            IsSuccessful = true,
            Token = token
        };
    }

}
 