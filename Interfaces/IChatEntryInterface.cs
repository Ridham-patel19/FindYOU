namespace FindYOU;

public interface IChatEntryInterface
{
 IEnumerable<ChatEntry> GetAll();

    ChatEntry? GetById(int id);

    IEnumerable<ChatEntry> GetByCategory(int id);

    void Add(ChatEntry chatEntry);

    void Update(ChatEntry chatEntry);

    void Delete(int id);

    void Save();
}
