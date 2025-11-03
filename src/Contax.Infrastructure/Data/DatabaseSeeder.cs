using Contax.Domain.Entities;
using Contax.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contax.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedDataAsync()
    {
        _logger.LogInformation("Iniciando processo de Seeding de dados.");

        // 1. Aplica Migrations pendentes
        try
        {
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao aplicar Migrations.");
            return;
        }

        // 2. Adiciona dados de teste
        if (!_context.Contacts.Any())
        {
            var contatosIniciais = new List<Contact>
            {
                // Apenas 3 argumentos: Nome, Email, Telefone
                new Contact("Aline Ferreira", "aline.ferreira@contato.com.br", "+5511987654321"),
                new Contact("Bruno Gonçalves", "bruno.g@contato.com.br", "+5521998877665"),
                new Contact("Camila Ribeiro", "camila.ribeiro@contato.com.br", "+5531955443322"),
                new Contact("Daniel Santos", "daniel.santos@contato.com.br", "+5571912345678"),
                new Contact("Eduarda Costa", "eduarda.c@contato.com.br", "+5585901010101"),
                new Contact(
                    "Fernando Oliveira Junior",
                    "fernando.jr@empresa.com",
                    "+5561923456789"
                ),
                new Contact("Giovana Lima", "giovana.lima@negocios.com", "+5541934567890"),
                new Contact("Henrique Almeida", "henrique.almeida@tech.com", "+5551945678901"),
                new Contact("Isabela Rocha", "isabela.rocha@dev.com", "+5583956789012"),
                new Contact("Júlio Cezar", "julio.cezar@marketing.com", "+5592967890123"),
                new Contact("Larissa Martins", "larissa.m@mail.com", "+5511978901234"),
                new Contact("Marcelo Vieira", "marcelo.v@site.com", "+5521989012345"),
                new Contact("Natália Gomes", "natalia.gomes@info.net", "+5547990123456"),
                new Contact("Otávio Nogueira", "otavio.n@app.io", "+5562912340000"),
                new Contact("Patrícia Fernandes", "patricia.f@contato.com", "+5519923451111"),
                new Contact("Quênia Zaror", "kenia.z@teste.com", "+5581934562222"),
                new Contact("Renato Souza", "renato.souza@teste.com", "+5591945673333"),
                new Contact("Sofia Medeiros", "sofia.m@teste.com", "+5548956784444"),
                new Contact("Thiago Becker", "thiago.b@teste.com", "+5553967895555"),
                new Contact("Vanessa Queiroz", "vanessa.q@teste.com", "+5582978906666"),
            };
            // ... restante do seeder

            await _context.Contacts.AddRangeAsync(contatosIniciais);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Adicionados {Count} contatos iniciais com sucesso.",
                contatosIniciais.Count
            );
        }
        else
        {
            _logger.LogInformation(
                "O banco de dados já contém dados. Seeding de contatos ignorado."
            );
        }

        // ... Você pode adicionar seeders para outras entidades aqui (Ex: Usuários de Teste)
    }
}
