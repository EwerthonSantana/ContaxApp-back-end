using MediatR;

namespace Contax.Application.Contacts.Commands;

public record CreateContactCommand(string Name, string Email, string Phone) : IRequest<Guid>;
