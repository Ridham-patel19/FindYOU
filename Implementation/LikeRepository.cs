
using Microsoft.EntityFrameworkCore;

namespace FindYOU;

public class LikeRepository : ILikeInterface
{

     private readonly ApplicationDbContext _context;

    public LikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddLikeAsync(Like like)
    {
        await  _context.Likes.AddAsync(like);
    }

  public async Task DeleteLikeAsync(int chatId, int userId)
{
    var like = await _context.Likes
        .FirstOrDefaultAsync(l => l.ChatEntryId == chatId &&
                                  l.UserId == userId);

    if (like != null)
    {
        _context.Likes.Remove(like);
    }
}

    public async Task<int> GetLikeCountByChatAsync(int chatId)
    {
        return await _context.Likes.CountAsync(x => x.ChatEntryId == chatId);
    }

    public async Task<bool> HasUserLikedChatAsync(int chatId, int userId)
    {
        return  await _context.Likes.AnyAsync(x => x.ChatEntryId == chatId && x.UserId == userId);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
     }
}
