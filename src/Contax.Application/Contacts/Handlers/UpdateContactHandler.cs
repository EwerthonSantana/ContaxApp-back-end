using Contax.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, bool>
{
    private readonly IContactRepository _repository;

    public UpdateContactHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        UpdateContactCommand request,
        CancellationToken cancellationToken
    )
    {
        var contact = await _repository.GetByIdAsync(request.Id);

        if (contact == null)
        {
            return false;
        }

        string normalizedPhone = PhoneNumberUtility.Normalize(request.Phone);

        // 3. REGRA DE NEGÓCIO: Validação de Unicidade
        // 🚨 Ponto CRÍTICO: Passamos o ID do contato atual (request.Id).
        // A implementação no Repositório irá gerar: WHERE Phone = @normalizedPhone AND Id != @request.Id
        if (await _repository.ExistsByPhoneAsync(normalizedPhone, request.Id))
        {
            throw new ValidationException(
                "Validation failed",
                [
                    new ValidationFailure(
                        "Phone",
                        "Este número de telefone já está sendo usado por outro contato no sistema."
                    ),
                ]
            );
        }

        contact.Update(request.Name, request.Email, normalizedPhone);

        await _repository.UpdateAsync(contact);

        return true;
    }
}
