// Contax.Application/Handlers/UpdateContactHandler.cs
using System.Threading;
using System.Threading.Tasks;
using Contax.Domain.Interfaces;
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
            return false; // Ou lançar uma exceção de "Não Encontrado"
        }

        // Chamando o método de domínio para atualizar o estado da Entidade (DDD)
        contact.Update(request.Name, request.Email, request.Phone);

        await _repository.UpdateAsync(contact);

        return true;
    }
}
