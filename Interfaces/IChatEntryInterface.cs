namespace FindYOU;

public interface IChatEntryInterface
{
 IEnumerable<ChatEntry> GetAll(int? userid);

    ChatEntry? GetById(int id , int? userid);

    List<ChatEntry> GetByCategory(int id , int? userid);

    void Add(ChatEntry chatEntry);

    void Update(ChatEntry chatEntry);

    void Delete(int id);

    void Save();
}
