using Contax.Domain.Interfaces;
using MediatR;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly IContactRepository _repository;

    public DeleteContactHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        DeleteContactCommand request,
        CancellationToken cancellationToken
    )
    {
        var contact = await _repository.GetByIdAsync(request.Id);

        if (contact == null)
        {
            return false;
        }

        await _repository.DeleteAsync(contact);

        return true;
    }
}
