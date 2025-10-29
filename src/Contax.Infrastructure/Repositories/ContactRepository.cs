using System.Linq.Expressions;
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using Contax.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contax.Infrastructure.Repositories;

// Agenda.Infrastructure/Repositories/ContatoRepository.cs
public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    // Opcional: private readonly IDbConnection _dapperConnection;

    public ContactRepository(AppDbContext context) //, IDbConnection dapperConnection
    {
        _context = context;
        // _dapperConnection = dapperConnection;
    }

    public async Task AddAsync(Contact contato)
    {
        await _context.Contacts.AddAsync(contato);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Contact>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Contact> GetByIdAsync(Guid id)
    {
        // Exemplo Dapper (Opcional: para Read Model em CQRS)
        // var sql = "SELECT * FROM Contatos WHERE Id = @Id";
        // return await _dapperConnection.QuerySingleOrDefaultAsync<Contato>(sql, new { Id = id });

        // Exemplo EF Core (Pode ser usado para Write Model)
        return await _context.Contacts.FindAsync(id);
    }

    public Task UpdateAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    // ... Implementar GetAllAsync, UpdateAsync, DeleteAsync usando _context
}
