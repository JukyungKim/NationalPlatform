using Microsoft.AspNetCore.Mvc;
using NationalPlatform.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;

namespace NationalPlatform.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AccountController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Route("home/main/account")]
    public IActionResult Index()
    {
        Console.WriteLine("Sensor index");
        return View("/views/home/main/account/account.cshtml");
    }
    [Route("home/main/account/add")]
    public IActionResult Add()
    {
        Console.WriteLine("Add sensor index");
        return View("/views/home/main/account/account.cshtml");
    }

    [HttpPost]
    public IActionResult RegistAccount(string id,
    string password, string name, string authority, string phone_number, string district)
    {
        Console.WriteLine("Regist sensor: {0} {1} {2} {3} {4} {5}", id,
        password, name, authority, phone_number, district);
        RegistAccountModel.InsertAccount(id, password, name, authority, phone_number, district);
        RegistAccountModel.ReadAccount();

        return NoContent();
    }
}


