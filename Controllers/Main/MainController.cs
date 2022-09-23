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
    // [Route("home/main")]
    public IActionResult Index()
    {
        Console.WriteLine("Main page");
        return View("/views/home/main/main.cshtml");
    }

    // [Route("home/main/registsensor")]
    public IActionResult RegistSensor()
    {
        Console.WriteLine("RegistSensor");
        return View("/views/home/main/sensor/RegistSensor.cshtml");
    }
    
    public IActionResult RegistPlane()
    {
        return View("/views/home/main/plane/RegistPlane.cshtml");
    }
    public IActionResult RegistAccount()
    {
        return View("/views/home/main/account/RegistAccount.cshtml");
    }
}

