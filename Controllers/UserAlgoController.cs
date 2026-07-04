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

    // Basic recommendations
    var chats = await _algoRepo.GetRecommendedChatsAsync(userId.Value);

    // Vector recommendations (contains LikeCount, IsLiked, IsBookmarked)
    var query = string.IsNullOrWhiteSpace(user.InterestTags)
        ? "General"
        : user.InterestTags;

    var vectorChats = await _algoRepo.GetVectorFeedAsync(userId.Value, query);
Console.WriteLine($"Vector Chat Count = {vectorChats.Count}");
    foreach (var item in vectorChats)
{
       System.Console.WriteLine(item.LikeCount);
}

    // Update basic chats with values from vector feed if they exist
    foreach (var chat in chats)
    {
        var vectorChat = vectorChats.FirstOrDefault(x => x.Id == chat.Id);

        if (vectorChat != null)
        {
            chat.IsBookmarked = vectorChat.IsBookmarked;
            chat.IsLiked = vectorChat.IsLiked;
            chat.LikeCount = vectorChat.LikeCount;
        }
    }

    // Add chats that exist only in vector feed
    foreach (var vectorChat in vectorChats)
    {
        if (!chats.Any(x => x.Id == vectorChat.Id))
        {
            chats.Add(vectorChat);
        }
    }

    return Ok(chats);
}
}
}
