
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NationalPlatform.Models;

class Test : PageModel
{
    public int getValue = 0;
    public int postValue = 0;
    public void razortest(){
        Console.WriteLine("Razor test");
    }
}