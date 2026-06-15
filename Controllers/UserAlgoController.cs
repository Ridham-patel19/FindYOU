using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class UserAlgoController : Controller
    {

        private readonly IUserAlgoInterface _algoRepo;

        public UserAlgoController(IUserAlgoInterface algo)
        {
            _algoRepo = algo;
        }


        public IActionResult Index()
        {
            return View();
        }
        // GET: UserAlgoController
      public async Task<IActionResult> GetUserRecommendationBasic()
        {
            int? userid = HttpContext.Session.GetInt32("Userid");
            int id = (int)userid;
            var chats = await _algoRepo.GetRecommendedChatsAsync(id);

            return Ok(chats);
        }

    }
}
