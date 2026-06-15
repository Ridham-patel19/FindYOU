
using Microsoft.EntityFrameworkCore;

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

    public IEnumerable<Category> GetAll(int userId)
    {
        
        return _context.Categories.Where(x=> x.UserId == userId).ToList();
    }

    public Category? GetById(int id , int userId)
    {
        return _context.Categories
    .FirstOrDefault(x => x.Id == id && x.UserId == userId);
    }

    public void Save()
    {
           _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

public bool IsEligible(int categoryId, int userId)
{
    return _context.Categories
        .Any(x => x.Id == categoryId && x.UserId == userId);
}
    
}
