
namespace FindYOU;

public class CategoryRepository : ICategoryInterface
{

    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(Category category)
    {
        _context.Categories.Add(category);
    }

    public void Delete(int id)
    {
         var category = _context.Categories.Find(id);

        if (category != null)
        {
            _context.Categories.Remove(category);
        }
    }

    public IEnumerable<Category> GetAll()
    {
        
        return _context.Categories.ToList();
    }

    public Category? GetById(int id)
    {
        return _context.Categories.Find(id);
    }

    public void Save()
    {
           _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    
}
