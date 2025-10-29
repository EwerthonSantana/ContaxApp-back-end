// Contax.Application/Queries/GenerateTokenQuery.cs

using MediatR;

/// <summary>
/// Query para solicitar a geração de um Token JWT após validação das credenciais.
/// Retorna o token como uma string.
/// </summary>
public class GenerateTokenQuery : IRequest<string>
{
    public string Username { get; set; }
    public string Password { get; set; }

    // Construtor opcional, se preferir inicializar via método
    public GenerateTokenQuery(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
