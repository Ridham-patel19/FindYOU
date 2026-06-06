
namespace FindYOU;

public class AuthRepository : IAuthInterface
{

     private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public int Login(string email, string password)
{
    bool exists = _context.Users
        .Any(x => x.Email == email && x.password == password);

    return exists ? 1 : 0;
}
}
