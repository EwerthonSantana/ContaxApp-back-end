using System.Linq.Expressions;
using Contax.Domain.Entities;

namespace Contax.Domain.Interfaces;

public interface IContactRepository
{
    Task<Contact> GetByIdAsync(Guid id);
    Task<IEnumerable<Contact>> GetAllAsync();
    Task AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Contact contact);
}
