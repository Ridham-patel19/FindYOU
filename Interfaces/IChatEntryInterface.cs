namespace FindYOU;

public interface IChatEntryInterface
{
 IEnumerable<ChatEntry> GetAll(int? userid);

    ChatEntry? GetById(int id , int? userid);

    List<ChatEntry> GetByCategory(int id , int? userid);

    public bool IsEligible(int chatId, int userId);

    void Add(ChatEntry chatEntry);

    void Update(ChatEntry chatEntry);

    int UpdateChatAccess(int id , bool isPublic);

    void Delete(int id);

    void Save();
}
