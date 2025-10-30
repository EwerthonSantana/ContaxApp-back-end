namespace Contax.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Contact() { }

    public Contact(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome é obrigatório.", nameof(name));

        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }
}
