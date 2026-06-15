namespace FindYOU;

public interface ICategoryInterface
{
IEnumerable<Category> GetAll(int userId);

    Category? GetById(int id , int userId);

    void Add(Category category);

    void Update(Category category);

    void Delete(int id );

    public bool IsEligible(int categoryId, int userId);
       void Save();
}
