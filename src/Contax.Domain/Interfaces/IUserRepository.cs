public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ValidatePasswordAsync(string providedPassword, string storedPasswordHash);
}
