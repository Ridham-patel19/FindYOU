namespace FindYOU;

public interface ICategoryInterface
{
IEnumerable<Category> GetAll();

    Category? GetById(int id);

    void Add(Category category);

    void Update(Category category);

    void Delete(int id);

    void Save();
}
