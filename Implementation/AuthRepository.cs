
namespace FindYOU;

public class AuthRepository : IAuthInterface
{

     private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public User? Login(string email, string password)
{
 User? user = _context.Users
    .FirstOrDefault(x => x.Email == email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password))
    {
        return null;
    }

    return user;
}

    public int Register(User user)
    {

       bool isEmailExist =  _context.Users.Any(x=> x.Email == user.Email);

        if (isEmailExist)
        {
            return -1;
        }
        user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

        _context.Users.Add(user);

int rowsAffected = _context.SaveChanges();

if(rowsAffected <= 0)
{
    return 0;
}

return 1;
    }
}
