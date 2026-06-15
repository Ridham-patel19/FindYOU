
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

    public bool IsEligible(int chatId, int userId)
    {
       return _context.ChatEntries.Any(x => x.Id == chatId && x.UserId == userId);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(ChatEntry chatEntry)
    {
        _context.ChatEntries.Update(chatEntry);
    }

public int UpdateChatAccess(int id , bool isPublic)
    {
       ChatEntry chat = new ChatEntry
       {
           Id = id,
           IsPublic = isPublic
       };

System.Console.WriteLine(isPublic);
//its just store meta dat of this obj its in DB 
          _context.ChatEntries.Attach(chat);
   _context.Entry(chat).Property(x => x.IsPublic).IsModified = true;

   int rows =  _context.SaveChanges();

   System.Console.WriteLine(rows);

   return rows>0 ? 1 : 0;


    }
    
}
