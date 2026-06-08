using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ChatEntryController : Controller
    {


       private  int? UserId ;
        private readonly IChatEntryInterface _repo;

        public ChatEntryController(IChatEntryInterface repo)
        {
           _repo = repo;
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
         public IActionResult Create(ChatEntry chat)
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

                chat.UserId = id;
                _repo.Add(chat);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ChatEntry chat)
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


    }
}
