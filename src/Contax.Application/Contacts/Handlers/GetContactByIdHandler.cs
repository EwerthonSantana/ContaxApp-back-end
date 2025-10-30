// Agenda.Application/Handlers/GetContactByIdHandler.cs
using System.Data;
using AutoMapper;
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using Dapper;
using MediatR;

public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactDTO>
{
    private readonly IDbConnection _dbConnection;
    private readonly IMapper _mapper;

    public GetContactByIdHandler(IDbConnection dbConnection, IMapper mapper)
    {
        _dbConnection = dbConnection;
        _mapper = mapper;
    }

    public async Task<ContactDTO> Handle(
        GetContactByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        const string sql =
            "SELECT \"Id\", \"Name\", \"Email\", \"Phone\" FROM \"Contacts\" WHERE \"Id\" = @Id";
        var Contact = await _dbConnection.QuerySingleOrDefaultAsync<Contact>(
            sql,
            new { request.Id }
        );

        return _mapper.Map<ContactDTO>(Contact);
    }
}
