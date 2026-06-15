using System.Threading.Tasks;
using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ChatEntryController : Controller
    {


       private  int? UserId ;
        private readonly IChatEntryInterface _repo;

        private readonly AITagsGeneration _AiTags;
                private readonly ICategoryInterface _catrepo;


        public ChatEntryController(IChatEntryInterface repo,ICategoryInterface catrepo , AITagsGeneration AiTags)
        {
           _repo = repo;
           _catrepo = catrepo;
           _AiTags = AiTags;
        }
        // GET: ChatEntryController
      public IActionResult Index(int? categoryId)
{

     int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
    IEnumerable<ChatEntry> chats;
  

    if(categoryId.HasValue)
    {
        chats = _repo.GetByCategory(categoryId.Value , UserId);
        return View(chats);
    }
    else
    {
         chats = _repo.GetAll(UserId);
    }

    return View(chats);
}

        public IActionResult Detail(int id)
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
            var entry = _repo.GetById(id , UserId);

            if(entry == null)
            {
                return NotFound();
            }
            return Ok(entry);
        }

        public IActionResult Create()
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }

           
            return View();
        }

        [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(ChatEntry chat)
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }else if(UserId == null)
            {
                return RedirectToAction("Login" , "Home");
            }
             if (ModelState.IsValid)
            {
                int id = (int)UserId;



                 bool iseligible = _catrepo.IsEligible(chat.CategoryId , id);

                 System.Console.WriteLine("Categiry id " + chat.CategoryId);
                 System.Console.WriteLine("User id : " + id);
                 System.Console.WriteLine("is eligible result : " + iseligible);

                if (!iseligible)
                {
                    return Unauthorized(new
                    {
                        message="this category doesnot belongs to you"
                    });
                }

                 chat.Category =  _catrepo.GetById(chat.CategoryId , id);
                


                 chat.ChatTags = await _AiTags.GenerateTagsAsync(chat.Title , chat.Category.Name , chat.Summary , chat.Notes);
                chat.UserId = id;
                _repo.Add(chat);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, ChatEntry chat)
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }else if(UserId == null)
            {
                return RedirectToAction("Login" , "Home");
            }
            if (id != chat.Id)
                return NotFound();

            if (ModelState.IsValid)
            {

                  id = (int)UserId;
   bool iseligible = _catrepo.IsEligible(chat.CategoryId , id);

                if (!iseligible)
                {
                    return Unauthorized(new
                    {
                        message="this category doesnot belongs to you"
                    });
                }


                 chat.Category =  _catrepo.GetById(chat.CategoryId , id);
                


                 chat.ChatTags = await _AiTags.GenerateTagsAsync(chat.Title , chat.Category.Name , chat.Summary , chat.Notes);
                chat.UserId = id;
                _repo.Update(chat);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(chat);
        }

        
         [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }else if(UserId == null)
            {
                return RedirectToAction("Login" , "Home");
            }


            // int userid = (int)UserId;

            //    bool iseligible = _catrepo.IsEligible(id , userid);

            //     if (!iseligible)
            //     {
            //         return Unauthorized(new
            //         {
            //             message="this category doesnot belongs to you"
            //         });
            //     }
            _repo.Delete(id);
            _repo.Save();

            return RedirectToAction(nameof(Index));
        }

       
        public IActionResult GetByCategory(int id)
        {

            int result = CheckAuth();

            if(result == 0)
            {

                return RedirectToAction("Login" , "Home");
            }else if(UserId == null)
            {
                return RedirectToAction("Login" , "Home");
            }
           var chats =  _repo.GetByCategory(id , UserId);

         if (!chats.Any())
{
    return NotFound();
}

            return Ok(chats);

        }

         public int CheckAuth()
        {
            
            int? result = HttpContext.Session.GetInt32("Userid");
            string? role = HttpContext.Session.GetString("Role");


            if(result.HasValue && role == "User")
            {
                UserId = result;
                return 1;
            }
            else
            {
                return 0;
            }
        }

[HttpPost]
public async Task<IActionResult> UpdateChatAccess(int chatid , bool ispublic)
        {

              int results = CheckAuth();

            if(results == 0)
            {

                return RedirectToAction("Login" , "Home");
            }else if(UserId == null)
            {
                return RedirectToAction("Login" , "Home");
            }
                 int userid = (int)UserId;
            bool iseligible = _repo.IsEligible(chatid , userid);

            if (!iseligible)
            {
                return BadRequest(new
                {
                    success = false ,
                    message = "You cant manipulate this ChatENtry"
                });
            }
           var result =  _repo.UpdateChatAccess(chatid , ispublic);

           if(result <= 0)
            {
                return BadRequest(new
                {
                     success = false ,
                    message = "error while updating please try again "
                });
            }

            return Ok(new
            {
                success = true,
                message = "IsPublic Updated successfully"
            });
        }

    }
}
