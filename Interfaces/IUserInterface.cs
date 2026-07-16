using FindYOU;

namespace FindYOU;

public interface IUserInterface
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
}
