using Microsoft.AspNetCore.Mvc;
using NationalPlatform.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;

namespace NationalPlatform.Controllers;

public class PlanController : Controller
{
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
        long size = files.Sum(f => f.Length);

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                var filePath = Path.GetTempFileName();
                Console.WriteLine(filePath);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
        }
        return Ok(new { count = files.Count, size });
    }

    [HttpPost]
    public IActionResult UploadImage(IFormFile file, string plan_image_name, 
        string address, string building_name, string dong, string floor, string ho)
    {
        if (file != null)
        {
            bool result = RegistPlanModel.CheckPlanId(plan_image_name);
            if(result == false){
                string imagePath;
                Console.WriteLine("Image Upload");
                Console.WriteLine(file.FileName);
                RegistPlanModel.InsertImage(file, address, plan_image_name, building_name, dong, floor, ho);
                imagePath = RegistPlanModel.ReadImage(plan_image_name);
                ViewData["PlanImage"] = imagePath;
            }
            
        }        

        return View("/views/home/main/plane/RegistPlane.cshtml");
        // return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Post(IFormFile file)
    {
        //process file 
        Console.WriteLine("Image Upload");
        Console.WriteLine(file.Length);
        Console.WriteLine(file.FileName);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        Console.WriteLine(file.FileName);
        if (file == null || file.Length == 0)
            return Content("file not selected");

        var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot",
                    file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return NoContent();
    }

    [HttpPost]
    public IActionResult RemovePlan(string planName)
    {
        Console.WriteLine("Remove plan name : {0}", planName);
        RegistPlanModel.RemovePlan(planName);
        return Redirect("/main/registPlane");
    }
}

