
using Microsoft.EntityFrameworkCore;

namespace FindYOU;

public class ChatEntryRepository : IChatEntryInterface
{

       private readonly ApplicationDbContext _context;

    public ChatEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(ChatEntry chatEntry)
    {
        _context.ChatEntries.Add(chatEntry);
    }

    public void Delete(int id)
    {
        var chat = _context.ChatEntries.Find(id);

        if(chat != null)
        {
            _context.ChatEntries.Remove(chat);
        }
    }

    public IEnumerable<ChatEntry> GetAll(int? userid)
    {
        
        return _context.ChatEntries.Include(x => x.Category).Where(x => x.UserId == userid).ToList();
    }

   public List<ChatEntry> GetByCategory(int id , int? userid)
{
    return _context.ChatEntries
                   .Include(x => x.Category)
                   .Where(x => x.CategoryId == id && x.UserId == userid )
                   .ToList();
}

    public ChatEntry? GetById(int id , int? userid)
    {
        return _context.ChatEntries
        .Include(x => x.Category)
        .FirstOrDefault(x => x.Id == id && x.UserId == userid);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(ChatEntry chatEntry)
    {
        _context.ChatEntries.Update(chatEntry);
    }


    
}
