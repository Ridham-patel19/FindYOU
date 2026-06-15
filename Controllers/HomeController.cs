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

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if(user == null || !ModelState.IsValid)
        {
            ViewBag.Error = "Error while register";
            return View();
        }

        int result = _auth.Register(user);

        if(result == -1)
        {
            ViewBag.Error = "Email Already Exist !!";
            return View();
        }
        else if(result == 0)
        {
             ViewBag.Error = "Error while register";
            return View();
        }
        else
        {
             return RedirectToAction("Login");
        }


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
        User result = _auth.Login(email , password);


        System.Console.WriteLine(result.Id + result.Role);

        if(result == null)
        {
            ViewBag.Error = "Invalid email or password";
            return View();
        }
        else
        {

            HttpContext.Session.SetInt32("Userid" , result.Id);
            HttpContext.Session.SetString("Role" , result.Role);

            return RedirectToAction("Index" , "ChatEntry");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
