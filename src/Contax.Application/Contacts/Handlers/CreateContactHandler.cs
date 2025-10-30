using Contax.Application.Contacts.Commands;
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Contax.Application.Contacts.Handlers;

public class CreateContactHandler : IRequestHandler<CreateContactCommand, Guid>
{
    private readonly IContactRepository _repository;

    public CreateContactHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CreateContactCommand request,
        CancellationToken cancellationToken
    )
    {
        string normalizedPhone = PhoneNumberUtility.Normalize(request.Phone);

        if (await _repository.ExistsByPhoneAsync(normalizedPhone))
        {
            throw new ValidationException(
                "Validation failed",
                [new ValidationFailure("Phone", "Este número de telefone já está cadastrado.")]
            );
        }

        var contact = new Contact(request.Name, request.Email, request.Phone);

        await _repository.AddAsync(contact);

        return contact.Id;
    }
}
