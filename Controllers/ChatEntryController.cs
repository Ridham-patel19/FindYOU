using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ChatEntryController : Controller
    {

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
        chats = _repo.GetByCategory(categoryId.Value);
    }
    else
    {
        chats = _repo.GetAll();
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
            var entry = _repo.GetById(id);

            if(entry == null)
            {
                return NotFound();
            }
            return View(entry);
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
            }
             if (ModelState.IsValid)
            {
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
            }
            _repo.Delete(id);
            _repo.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult GetByCategory(int id)
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
           var chats =  _repo.GetByCategory(id);

         if (!chats.Any())
{
    return NotFound();
}

            return Ok(chats);

        }

         public int CheckAuth()
        {
            
            string? result = HttpContext.Session.GetString("User");


            if(result == "Ridham")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


    }
}
