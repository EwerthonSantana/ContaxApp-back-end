using System;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = default!;

    // Construtor sem parâmetros, necessário para o EF Core
    private User() { }

    public User(string username, string passwordHash, string role)
    {
        // Regras de domínio:
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username é obrigatório.");

        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }

    // Método de Domínio para atualizar a senha (comportamento)
    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("PasswordHash é obrigatório.");
        PasswordHash = newPasswordHash;
    }
}
