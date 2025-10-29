using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Contax.Domain.Interfaces;
using MediatR;

public class GetAllContactsHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDTO>>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public GetAllContactsHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDTO>> Handle(
        GetAllContactsQuery request,
        CancellationToken cancellationToken
    )
    {
        var contacts = await _repository.GetAllAsync();

        // Mapeia a lista de Entidades para uma lista de DTOs
        return _mapper.Map<IEnumerable<ContactDTO>>(contacts);
    }
}
