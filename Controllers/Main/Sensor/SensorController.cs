using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using NationalPlatform.Models;

namespace NationalPlatform.Controllers;

public class SensorController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public SensorController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Route("home/main/sensor")]
    public IActionResult Index()
    {
        Console.WriteLine("Sensor index");
        return View("/views/home/main/sensor/index.cshtml");
    }
    [Route("home/main/sensor/add")]
    public IActionResult Add()
    {
        Console.WriteLine("Add sensor index");
        return View("/views/home/main/sensor/add.cshtml");
    }

    [HttpPost]
    public IActionResult RegistSensor(string sensor_id,
    string address, string building_name, string dong, string floor, string ho,
    string number)
    {
        Console.WriteLine("Regist sensor: {0} {1} {2} {3} {4} {5} {6}",sensor_id,
        address, building_name, dong, floor, ho, number);
        // RegistSensorModel.InsertSensorInfo(sensor_id, address, building_name, dong, floor, ho, number);
        RegistSensorModel.ReadSensorInfo();

        return NoContent();
    }
}
