using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class UserAlgoController : Controller
    {

        private readonly IUserAlgoInterface _algoRepo;

        private readonly IUserInterface _userRepo;

        public UserAlgoController(IUserAlgoInterface algo , IUserInterface userRepo)
        {
            _algoRepo = algo;
            _userRepo = userRepo;
        }


        public IActionResult Index()
        {
             var userId = HttpContext.Session.GetInt32("Userid");

    if (userId == null)
        return RedirectToAction("Login" , "Home");
            return View();
        }
        // GET: UserAlgoController
    public async Task<IActionResult> GetUserRecommendationBasic()
{
    var userId = HttpContext.Session.GetInt32("Userid");

    if (userId == null)
        return Unauthorized();

    var user = await _userRepo.GetByIdAsync(userId.Value);

    if (user == null)
        return NotFound();
   
    var chats = await _algoRepo.GetRecommendedChatsAsync(userId.Value);
    // chats.ForEach(x => System.Console.WriteLine(x.Id));

    var query = string.IsNullOrWhiteSpace(user.InterestTags)
        ? "General"
        : user.InterestTags;

//         Console.WriteLine("Interest Tags:");
// Console.WriteLine(user.InterestTags);

// Console.WriteLine("Recommended Chats:");

// foreach (var chat in chats)
// {
//     Console.WriteLine($"{chat.Id} - {chat.Title}");
// }

    var vectorChats = await _algoRepo.GetVectorFeedAsync(userId.Value , query);

System.Console.WriteLine("vectore chats");
System.Console.WriteLine(vectorChats.Count());
vectorChats.ForEach(x => System.Console.WriteLine(x.Id));
    chats.AddRange(vectorChats);

    chats = chats
        .GroupBy(x => x.Id)
        .Select(g => g.First())
        .ToList();


System.Console.WriteLine("before search vectore");

        chats.ForEach(x => System.Console.WriteLine(x.Id));

    return Ok(chats);
}
    }
}
