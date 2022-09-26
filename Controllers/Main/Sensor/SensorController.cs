using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using NationalPlatform.Models;
using Microsoft.AspNetCore.SignalR;

namespace NationalPlatform.Controllers;

public class SensorController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public SensorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // [Route("home/main/sensor")]
    public IActionResult Index()
    {
        Console.WriteLine("Sensor index");
        return View("/views/home/main/sensor/index.cshtml");
    }
    // [Route("home/main/sensor/add")]
    public IActionResult Add()
    {
        Console.WriteLine("Add sensor index");
        return View("/views/home/main/sensor/add.cshtml");
    }

    [HttpPost]
    public IActionResult RegistSensor(string sensor_id,
    string address, string building_name, string dong, string floor, string ho,
    string number, string plan)
    {
        string x = "0";
        string y = "0";
        bool result;
        Console.WriteLine("Regist sensor: {0} {1} {2} {3} {4} {5} {6} {7}",sensor_id,
        address, building_name, dong, floor, ho, number, plan);
        result = RegistSensorModel.CheckSensorId(sensor_id);
        if(result == false){
            RegistSensorModel.InsertSensorInfo(sensor_id, address, building_name, dong, floor, ho, number, plan, x, y);
        }
        // RegistSensorModel.ReadSensorInfo();
        // return RedirectToAction("/home/main/registsensor");
        // return Redirect(Request.RawUrl);
        // return View("/views/home/main/sensor/registsensor.cshtml");
        // return RedirectToAction("Index");
        // return Redirect("/Sensor/RegistSensor"); 
        // return RedirectToAction("RegistSensor", "Main");
        return NoContent();

    }
    // [HttpPost]
    public IActionResult RemoveSensor(string sensorId)
    {
        Console.WriteLine("RemoveSensor Id : {0}", sensorId);
        RegistSensorModel.RemoveSensor(sensorId);
        // return RedirectToAction("Success");
        // return Redirect("/home/main/registsensor");
        return RedirectToAction("RegistSensor", "Main");
        // return NoContent();
        // return View("/views/home/main/sensor/registsensor.cshtml");
    }
}

public class SensorHub: Hub
{
    public async Task CheckSensorId(string id)
    {
        bool result;
        result = RegistSensorModel.CheckSensorId(id);
        Console.WriteLine("센서 id 체크 " + id + " " + result);
        await Clients.All.SendAsync("SensorId", result);
    }
}

