namespace Contax.Domain.Entities;

// Contax.Domain/Entities/Contact.cs (Corrigido para DDD)

public class Contact
{
    // Mude de { get; set; } para { get; private set; }
    public Guid Id { get; private set; } = Guid.NewGuid(); // Já inicializa
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Construtor private (para uso exclusivo do EF Core)
    private Contact() { }

    // Construtor PÚBLICO (o único caminho permitido para criar a Entidade)
    public Contact(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome é obrigatório.", nameof(name));

        // As propriedades são setadas APENAS no construtor ou por métodos de domínio.
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }

    // O método Update usa os private setters internos
    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }
}
