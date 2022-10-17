using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Diagnostics;

namespace NationalPlatform.Controllers;

public class MainController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public MainController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // [Route("/main")]
    public IActionResult Index()
    {
        if(!AccountController.isLogin){
            return NoContent();
        }
        Console.WriteLine("Main page");
        return View("/views/home/main/main.cshtml");
        // return View("/views/home/main/account/login.cshtml");
    }

    public IActionResult RegistSensorRe()
    {
        if(!AccountController.isLogin){
            return NoContent();
        }
        Console.WriteLine("RegistSensor");
        return View("/views/home/main/sensor/RegistSensor.cshtml");
    }
    
    public IActionResult RegistPlaneRe()
    {
        if(!AccountController.isLogin){
            return NoContent();
        }
        return View("/views/home/main/plane/RegistPlane.cshtml");
    }

    // [Route("home/main/registsensor")]
    public IActionResult RegistSensor()
    {
        Console.WriteLine("RegistSensor");
        return RedirectToAction("RegistSensorRe", "Main");
    }
    
    public IActionResult RegistPlane()
    {
        return RedirectToAction("RegistPlaneRe", "Main");
    }
    public IActionResult RegistAccount()
    {
        return View("/views/home/main/account/RegistAccount.cshtml");
    }
    public IActionResult Login()
    {
        AccountController.isLogin = false;
        return View("/views/home/main/account/Login.cshtml");
    }
    
}

