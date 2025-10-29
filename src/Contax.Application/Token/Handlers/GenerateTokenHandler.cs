// Contax.Application/Handlers/GenerateTokenHandler.cs
// ...
using MediatR;

public class GenerateTokenHandler : IRequestHandler<GenerateTokenQuery, string>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository; // NOVO: Injeção do Repositório

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
        // 1. Busca o usuário no repositório (DB)
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        // 2. Valida a senha (Regra de Negócio)
        var passwordIsValid = await _userRepository.ValidatePasswordAsync(
            request.Password,
            user.PasswordHash
        );

        if (!passwordIsValid)
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        // 3. Geração do Token com a Role
        var token = _tokenService.GenerateToken(user.Username, user.Role);

        return token;
    }
}
