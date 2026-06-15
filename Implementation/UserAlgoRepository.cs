

using Microsoft.EntityFrameworkCore;

namespace FindYOU;

public class UserAlgoRepository : IUserAlgoInterface
{

     private readonly ApplicationDbContext _context;

    public UserAlgoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatEntry>> GetRecommendedChatsAsync(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user == null || string.IsNullOrWhiteSpace(user.InterestTags))
        {
            return new List<ChatEntry>();
        }
        var interestList = user.InterestTags
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(tag => tag.ToLower())
            .ToList();

        var chats = await _context.ChatEntries
        .Include(c => c.Category)
        .Where(c =>
            c.IsPublic  == true &&
            c.ChatTags != null &&
             interestList.Any(tag =>
            c.ChatTags.ToLower().Contains(tag)))
        .ToListAsync();

        return chats;
    }
}

