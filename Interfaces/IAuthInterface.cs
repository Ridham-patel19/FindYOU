namespace FindYOU;

public interface IAuthInterface
{
    User Login(string email , string password);

    int Register(User user);
    
}
