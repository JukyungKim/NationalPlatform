using Microsoft.AspNetCore.Mvc;
using NationalPlatform.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.SignalR;

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

    [Route("home/login")]
    public IActionResult Login()
    {
        Console.WriteLine("Login page");
        return View("/views/home/main/account/login.cshtml");
    }

    public IActionResult CheckLogin(string id, string password)
    {
        Console.WriteLine("Check login : {0} {1}", id, password);

        int ok;
        ok = AccountModel.CheckAccount(id, password);

        if(ok == 0){
            return NoContent();
        }
        else if(ok == 1){
            return View("/views/home/main/main.cshtml");
        }
        else{
            return NoContent();
        }
    }

    public IActionResult ChangePasswordPage()
    {
        return View("/views/home/main/account/password.cshtml");
    }

    public IActionResult ChangePassword(string password1, string password2)
    {
        Console.WriteLine("Change password");
        bool hasChar = false;
        bool hasNum = false;

        if(string.IsNullOrEmpty(password1)){
            Console.WriteLine("Empty password");
            return NoContent();
        }

        string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialChArray = specialCh.ToCharArray();
        foreach (char ch in specialChArray) {
            if (password1.Contains(ch))
                hasChar = true;
        }

        string num = @"1234567890";
        char[] numArray = num.ToCharArray();
        foreach (char ch in numArray) {
            if (password1.Contains(ch))
                hasNum = true;
        }
        
        if (password1.Contains(" ")){
            Console.WriteLine("Exist space");
            return NoContent();
        }
        else if(password1.Length < 10 || password1.Length > 20){
            Console.WriteLine("Not length");
            return NoContent();
        }
        else if(!password1.Any(char.IsUpper)){
            Console.WriteLine("Not upper");
            return NoContent();
        }
        else if(!password1.Any(char.IsLower)){
            Console.WriteLine("Not lower");
            return NoContent();
        }
        else if(!hasChar){
            Console.WriteLine("Not char");
            return NoContent();
        }
        else if(!hasNum){
            Console.WriteLine("Not num");
            return NoContent();
        }
        else if(password1 != password2){
            Console.WriteLine("Not same password");
            return NoContent();
        }
        else{
            AccountModel.UpdatePassword("master", password1);
            return View("/views/home/main/account/login.cshtml");
        }
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

    public string SaveEmployeeRecord()
    {
        string res = "this is return value";
        // do here some operation  
        return res;
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCompany(string a)
    {
        Console.WriteLine("Delete company");
        // Do some stuff
        return Ok();
    }

    [HttpPost]
    public IActionResult Test()
    {
        return RedirectToAction("/views/home/main/account/login.cshtml");
    }
}

public class AccountHub: Hub
{
    public async Task LoginResult(string password)
    {
        int result;
        Console.WriteLine("Login result : " + password);

        result = AccountModel.CheckAccount("master", password);

        await Clients.All.SendAsync("LoginError", result);
    }
}


