// Agenda.Application/Handlers/GetContactByIdHandler.cs
using AutoMapper;
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using MediatR;

public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactDTO>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public GetContactByIdHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDTO> Handle(
        GetContactByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var Contact = await _repository.GetByIdAsync(request.Id);

        return _mapper.Map<ContactDTO>(Contact);
    }
}
