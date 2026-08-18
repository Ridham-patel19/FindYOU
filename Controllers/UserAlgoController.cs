using FindYOU;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace MyApp.Namespace
{
    public class UserAlgoController : Controller
    {

        private readonly IUserAlgoInterface _algoRepo;

        private readonly IUserInterface _userRepo;

          private readonly AITagsGeneration _AiService;

        public UserAlgoController(IUserAlgoInterface algo , IUserInterface userRepo , AITagsGeneration AiTags)
        {
            _algoRepo = algo;
            _userRepo = userRepo;
            _AiService = AiTags;
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
    // chats.ForEach(x => System.Console.WriteLine(x.Id));

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


System.Console.WriteLine("before search vectore");

        chats.ForEach(x => System.Console.WriteLine(x.Id));

    return Ok(chats);
}


[HttpGet]
public async Task<IActionResult> UpdateUserFeedTags()
        {
              var userId = HttpContext.Session.GetInt32("Userid");


    if (userId == null)
        return Unauthorized();

                string tags = await _algoRepo.GetUpdatedUserInterestTag((int)userId);

            if (string.IsNullOrWhiteSpace(tags))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "interest tags are empty"
                });
            }

            string updatedTags = await _AiService.GetUpdatedInterestTagForUser(tags);
            System.Console.WriteLine(updatedTags);

              if (string.IsNullOrWhiteSpace(updatedTags))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "interest tags are empty"
                });
            }

            return Ok(updatedTags);


            
        }


        [HttpGet]
        public async Task<IActionResult> GetViralChats()
        {
              var userId = HttpContext.Session.GetInt32("Userid");

    if (userId == null)
        return Unauthorized();

        var chats = await _algoRepo.GetViralChats();

        if(chats == null || chats.Count() == 0)
         return BadRequest();

       return Ok(chats);
         
        }

        
}
}
