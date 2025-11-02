using Contax.Application.Contacts.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }

    // READ (Todos)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllContactsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // READ (Por Id)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetContactByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("O ID na URL e o ID no corpo devem ser iguais.");
        }

        var result = await _mediator.Send(command);

        if (result == null)
        {
            return NotFound("Contato não encontrado para atualização.");
        }

        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteContactCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound("Contato não encontrado para exclusão.");
        }

        return NoContent();
    }
}
