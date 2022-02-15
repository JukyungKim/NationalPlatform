using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NationalPlatform.Models;

namespace NationalPlatform.Controllers;

public class TestController : Controller
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }
    [Route("test/")]
    public IActionResult Test()
    {
        Console.WriteLine("Test controller");
        return View("/views/test/test.cshtml");
    }

    [Route("test2/")]
    public IActionResult Test2()
    {
        Console.WriteLine("Test controller");
        return View("/views/test/test2.cshtml");
    }
    [Route("test3/")]
    public IActionResult Test3()
    {
        var model = new Test(){
            getValue = 10,
            postValue = 20
        };

        ViewData["TestModel"] = new Test(){
            getValue = 30,
            postValue = 40
        };

        return View("/views/test/test3.cshtml", model);
    }

    [HttpGet]
    public IActionResult TestGet(string GetValue)   // Html의 name과 변수명을 일치시킨다.
    {
        Console.WriteLine("Test Get : " + GetValue);

        return View("/views/test/test2.cshtml");
    }
    [HttpPost]
    public IActionResult TestPost(string PostValue) // Html
    {
        Console.WriteLine("Test Post : " + HttpContext.Request.Form["PostValue"] + " " + PostValue);
        return View("/views/test/test2.cshtml");
    }

    // 주소 : /test/test4
    public IActionResult Test4()
    {
        // return View();

        // return View("/views/test/test2.cshtml");
        return Redirect("/test2");
    }

    public IActionResult TestContent()
    {
        return Content("Test Content");
    }
    public IActionResult TestRedirect()
    {
        return Redirect("/home");
    }

    [Route("test5/")]
    public IActionResult Test5()
    {
 
        return View("/views/test/test5.cshtml");
    }
        [Route("test6/")]
    public IActionResult Test6()
    {
 
        return View("/views/test/test6.cshtml");
    }
    public void TestGetJavascript(string firstName, string lastName)
    {
        Console.WriteLine("Test Get Javascript: {0}, {1}", firstName, lastName);
    }
}
