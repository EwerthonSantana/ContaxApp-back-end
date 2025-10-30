using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
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
            return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
        }
    }
}
