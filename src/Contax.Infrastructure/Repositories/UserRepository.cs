// Contax.Infrastructure/Repositories/UserRepository.cs
using Contax.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public Task<bool> ValidatePasswordAsync(string providedPassword, string storedPasswordHash)
    {
        bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, storedPasswordHash);

        return Task.FromResult(isValid);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
