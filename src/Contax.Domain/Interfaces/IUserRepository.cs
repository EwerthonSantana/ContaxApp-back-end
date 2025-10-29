// Contax.Domain/Interfaces/IUserRepository.cs
using System.Threading.Tasks;
using Contax.Domain.Entities;

public interface IUserRepository
{
    // Método principal para autenticação
    Task<User> GetUserByUsernameAsync(string username);

    // Métodos para gestão de usuários
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);

    Task<bool> ValidatePasswordAsync(string providedPassword, string storedPasswordHash);
}
