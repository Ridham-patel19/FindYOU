using Microsoft.EntityFrameworkCore;
using FindYOU.Models;
using FindYOU;

namespace FindYOU.Repositories;

public class UserRepository : IUserInterface
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return null;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.password = user.password;
        existingUser.Role = user.Role;
        existingUser.InterestTags = user.InterestTags;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        email = email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return false;

        user.password = BCrypt.Net.BCrypt.HashPassword(newPassword);

        await _context.SaveChangesAsync();

        return true;
    }
}
