using MediatR;

public class GenerateTokenQuery : IRequest<string>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public GenerateTokenQuery(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
