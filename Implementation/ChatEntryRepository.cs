
using System.Threading.Tasks;
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

    public int AddBookMark(int chatId, int userId)
    {
        var bookmark = new Bookmark
        {
            ChatEntryId = chatId,
            UserId = userId
        };

        _context.Bookmarks.Add(bookmark);

       return  _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var chat = _context.ChatEntries.Find(id);

        if(chat != null)
        {
            _context.ChatEntries.Remove(chat);
        }
    }

   public int DeleteBookMark(int id)
{
    var chat = _context.Bookmarks.Find(id);

    if (chat == null)
        return 0;

    _context.Bookmarks.Remove(chat);

    int rowsAffected = _context.SaveChanges();

    return rowsAffected > 0?1:0;
}    public IEnumerable<ChatEntry> GetAll(int? userid)
    {
        
        return _context.ChatEntries.Include(x => x.Category).Where(x => x.UserId == userid).ToList();
    }

    public async Task<List<ChatEntry>> GetAllBookMarkeByUser(int userId)
    {
      var chats = await _context.Bookmarks
    .Where(b => b.UserId == userId)
    .Include(b => b.ChatEntry)
        .ThenInclude(c => c.Category)
    .Select(b => b.ChatEntry)
    .Where(c => c.IsPublic)
    .ToListAsync();
        

      return chats;
    }

    public async Task<Bookmark> GetBookMarkByUserAndChat(int chatid, int userid)
    {
        Bookmark? bookmark = await _context.Bookmarks.FirstOrDefaultAsync(x => x.ChatEntryId == chatid && x.UserId == userid);

        return bookmark;
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

    public async Task<bool> isBookMarked(int userid, int chatid)
    {
        return await _context.Bookmarks.AnyAsync(x => x.ChatEntryId == chatid && x.UserId == userid);
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

    public async Task UpdateSearchVector(int chatId)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE ""ChatEntries""
        SET search_vector =
            to_tsvector(
                'english',
                coalesce(""Title"", '') || ' ' ||
                coalesce(""Summary"", '') || ' ' ||
                coalesce(""ChatTags"", '')
            )
        WHERE ""Id"" = {chatId}
    ");
    }
}
