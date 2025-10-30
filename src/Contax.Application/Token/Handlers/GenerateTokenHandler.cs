using MediatR;

public class GenerateTokenHandler : IRequestHandler<GenerateTokenQuery, string>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public GenerateTokenHandler(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(
        GenerateTokenQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        var passwordIsValid = await _userRepository.ValidatePasswordAsync(
            request.Password,
            user.PasswordHash
        );

        if (!passwordIsValid)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        var token = _tokenService.GenerateToken(user.Username, user.Role);

        return token;
    }
}
