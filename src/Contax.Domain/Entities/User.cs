public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = default!;

    private User() { }

    public User(string username, string passwordHash, string role)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username é obrigatório.");

        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("PasswordHash é obrigatório.");
        PasswordHash = newPasswordHash;
    }
}
