using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using Contax.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contax.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Contact contato)
    {
        await _context.Contacts.AddAsync(contato);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Contact contact)
    {
        _context.Contacts.Remove(contact);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Contacts.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Contact> GetByIdAsync(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        return contact;
    }

    public async Task UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByPhoneAsync(string phone, Guid? excludeId = null)
    {
        var query = _context.Contacts.AsNoTracking().Where(c => c.Phone == phone);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
