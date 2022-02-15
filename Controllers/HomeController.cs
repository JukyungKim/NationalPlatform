using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalPlatform.Models;

namespace NationalPlatform.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Console.WriteLine("Home index");
        return View();
    }

    public IActionResult Privacy()
    {
        Console.WriteLine("Home privacy");
        return View();
    }

    public IActionResult Privacy2()
    {
        Console.WriteLine("Home privacy2");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
