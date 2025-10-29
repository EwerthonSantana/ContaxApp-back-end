// Contax.Api/Controllers/AuthController.cs

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data; // Certifique-se que este using está correto
using Microsoft.AspNetCore.Mvc;

[ApiController] // ESSENCIAL: Identifica a classe como um Controller de API
[Route("api/[controller]")] // ESSENCIAL: Define a rota base como /api/auth
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")] // ESSENCIAL: Define o método HTTP e a sub-rota /login
    [AllowAnonymous] // Permite que este método seja chamado sem token
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var query = new GenerateTokenQuery(request.Username, request.Password);
            var token = await _mediator.Send(query);

            return Ok(new { Token = token, Message = "Autenticação bem-sucedida" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Nome de usuário ou senha inválidos.");
        }
        catch (Exception ex)
        {
            // Opcional: Logar o erro completo aqui
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
}
