using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Contax.Domain.Entities; // NOVO
using Contax.Domain.Interfaces;
using Dapper;
using MediatR;

public class GetAllContactHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly IMapper _mapper;

    public GetAllContactHandler(IDbConnection dbConnection, IMapper mapper)
    {
        _dbConnection = dbConnection;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDTO>> Handle(
        GetAllContactsQuery request,
        CancellationToken cancellationToken
    )
    {
        const string sql =
            "SELECT \"Id\", \"Name\", \"Email\", \"Phone\" FROM \"Contacts\" ORDER BY \"Name\"";

        var contacts = await _dbConnection.QueryAsync<Contact>(sql);

        return _mapper.Map<IEnumerable<ContactDTO>>(contacts);
    }
}
