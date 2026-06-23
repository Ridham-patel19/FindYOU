namespace FindYOU;

public interface IChatEntryInterface
{
 IEnumerable<ChatEntry> GetAll(int? userid);

    ChatEntry? GetById(int id , int? userid);

    List<ChatEntry> GetByCategory(int id , int? userid);

    Task<List<ChatEntry>> GetAllBookMarkeByUser(int userId);

    Task<Bookmark> GetBookMarkByUserAndChat(int chatid , int userid);

    Task<bool> isBookMarked(int userid , int chatid);

    int DeleteBookMark(int id);

    public bool IsEligible(int chatId, int userId);

    int AddBookMark(int chatId , int userId);

    public Task UpdateSearchVector(int chatId);

    void Add(ChatEntry chatEntry);

    void Update(ChatEntry chatEntry);

    int UpdateChatAccess(int id , bool isPublic);

    void Delete(int id);

    void Save();
}
