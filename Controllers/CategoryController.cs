using FindYOU;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class CategoryController : Controller
    {

        private readonly ICategoryInterface _repo;

        public CategoryController(ICategoryInterface repo)
        {
            _repo = repo;
        }
        // GET: CategoryController
        public ActionResult Index()
        {

            int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
            var categories = _repo.GetAll();
            return View(categories);
        }

        public IActionResult GetAll()
        {

             int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
                        var categories = _repo.GetAll();
                  return Ok(categories);
        }

        public IActionResult Details(int id)
        {


             int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
            var category = _repo.GetById(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
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
        public IActionResult Create(Category category)
        {

             int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
            if (ModelState.IsValid)
            {
                _repo.Add(category);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {

             int result = CheckAuth();

            if(result == 0)
            {
                return RedirectToAction("Login" , "Home");
            }
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _repo.Update(category);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
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
