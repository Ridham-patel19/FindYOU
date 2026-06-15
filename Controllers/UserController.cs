using FindYOU;
using FindYOU.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class UserController : Controller
    {
        private int userId;
        private readonly IUserInterface _repo;

        public UserController(IUserInterface repo)
        {
            _repo = repo;
        }

        // GET: UserController
        public IActionResult Index()
        {
            int result = CheckAuth();

            if (result == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            return View("Index");
        }

        // GET: UserController/GetAll  -> Admin only
        public async Task<IActionResult> GetAll()
        {
            string? role = HttpContext.Session.GetString("Role");
            int? id = HttpContext.Session.GetInt32("Userid");

            if (role != "Admin" || id == null)
            {
                return StatusCode(403, new { message = "Access denied. Admins only." });
            }

            userId = (int)id;

            try
            {
                var users = await _repo.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving users.", error = ex.Message });
            }
        }

        // GET: UserController/Details  -> returns the logged-in user's own profile
        public async Task<IActionResult> Details()
        {
            int result = CheckAuth();

            if (result == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var user = await _repo.GetByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = ex.Message });
            }
        }

        // POST: UserController/Edit  -> updates the logged-in user's own profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            int result = CheckAuth();

            if (result == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUser = await _repo.UpdateAsync(userId, user);

                if (updatedUser == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }

        // POST: UserController/Delete  -> deletes the logged-in user's own account
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            int result = CheckAuth();

            if (result == 0)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var deleted = await _repo.DeleteAsync(userId);

                if (!deleted)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
            }
        }

        public int CheckAuth()
        {
            string? result = HttpContext.Session.GetString("Role");
            int? id = HttpContext.Session.GetInt32("Userid");

            if (result == "User" && id != null)
            {
                userId = (int)id;
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}