using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FindYOU.Models;

namespace FindYOU.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthInterface _auth;

    public HomeController(ILogger<HomeController> logger , IAuthInterface auth)
    {
        _logger = logger;
        _auth = auth;
    }

    public IActionResult Index()
    {
        return View();
    }



    // public IActionResult Privacy()
    // {
    //     return View();
    // }


    public IActionResult Login()
    {
         HttpContext.Session.Clear();
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult Login(string email , string password)
    {
        int result = _auth.Login(email , password);

        if(result == 0)
        {
            ViewBag.Error = "Invalid email or password";
            return View();
        }
        else
        {

            HttpContext.Session.SetString("User" , "Ridham");
            return RedirectToAction("Index" , "ChatEntry");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
