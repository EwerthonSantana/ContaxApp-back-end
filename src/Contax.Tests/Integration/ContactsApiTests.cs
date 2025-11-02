// Contax.Tests/Integration/ContactsApiTests.cs
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost; // Sua API
using Xunit;

// Usa a Factory customizada para injetar o DB In-Memory
public class ContactsApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContactsApiTests(CustomWebApplicationFactory<Program> factory)
    {
        // Cria o cliente HTTP para a API em memória
        _client = factory.CreateClient();
    }

    // --- UTILS: Simula o Fluxo de Autenticação para obter o Token ---

    private async Task<string> GetAuthTokenAsync(string username, string password)
    {
        var loginRequest = new { Username = username, Password = password };

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode(); // Deve retornar 200 OK

        var responseString = await response.Content.ReadAsStringAsync();
        var loginResponse = System.Text.Json.JsonDocument.Parse(responseString);

        // Retorna apenas a string do Token
        return loginResponse.RootElement.GetProperty("token").GetString()!;
    }

    // --- TESTES DE AUTORIZAÇÃO (JWT) ---

    [Fact]
    public async Task Contatos_DeveRetornar401_QuandoSemToken()
    {
        // Tenta acessar o endpoint sem o header Authorization
        var response = await _client.GetAsync("/api/contacts");

        // ASSERT: O Middleware JWT deve rejeitar
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Contatos_DeveRetornar200_QuandoComTokenValido()
    {
        // ARRANGE: 1. Obter o token (Simulando login com o usuário Admin do Seed Data)
        var token = await GetAuthTokenAsync("admin@contax.com", "senhaforte123");

        // ARRANGE: 2. Adiciona o token ao Header
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );

        // ACT: Acessa um recurso protegido (GET All)
        var response = await _client.GetAsync("/api/contacts");

        // ASSERT: 200 OK (A autenticação e autorização funcionaram)
        response.EnsureSuccessStatusCode();
    }

    // --- TESTE DE FLUXO CRUD COMPLETO ---

    [Fact]
    public async Task CreateContact_DeveRetornar400_QuandoTelefoneJaExiste()
    {
        // ARRANGE: Login e Token
        var token = await GetAuthTokenAsync("admin@contax.com", "senhaforte123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );

        // ARRANGE: Comando (telefones repetidos)
        var command1 = new
        {
            Name = "Contato A",
            Email = "a@a.com",
            Phone = "+5541900000001",
        };
        var command2 = new
        {
            Name = "Contato B",
            Email = "b@b.com",
            Phone = "+5541900000001",
        }; // Telefone repetido

        // ACT 1: Cria o primeiro contato (Deve ser 201 Created)
        var content1 = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(command1),
            Encoding.UTF8,
            "application/json"
        );
        var response1 = await _client.PostAsync("/api/contacts", content1);
        response1.EnsureSuccessStatusCode();

        // ACT 2: Tenta criar o segundo contato com o mesmo telefone
        var content2 = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(command2),
            Encoding.UTF8,
            "application/json"
        );
        var response2 = await _client.PostAsync("/api/contacts", content2);

        // ASSERT: Deve retornar 400 Bad Request devido à validação de unicidade
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

        // Opcional: verificar o corpo da resposta JSON para a mensagem de erro específica.
    }
}
