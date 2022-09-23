using System.Net;
using NationalPlatform.Models;
using Microsoft.AspNetCore.Connections;
using NationalPlatform.Controllers;
using Microsoft.AspNetCore;

Console.WriteLine("National platform starts");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddServerSideBlazor(); // razor
builder.Services.AddSignalR();

builder.WebHost.UseUrls();

builder.WebHost.ConfigureServices(services =>
{
    // services.AddFramework(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 5000));
    services.AddFramework(new IPEndPoint(IPAddress.Loopback, 5002));
}).UseKestrel(options =>
{
    options.ListenLocalhost(5003, build =>
    {
        build.UseConnectionHandler<TcpConnectionHandlers>();
    });
    options.Listen(IPAddress.Parse("0.0.0.0"), 5000);
    options.Listen(IPAddress.Parse("0.0.0.0"), 5001, builder =>
    {
        builder.UseHttps();
    });

    // // HTTP 5000
    // options.ListenLocalhost(5000);

    // // HTTPS 5001
    // options.ListenLocalhost(5001, builder =>
    // {
    //     builder.UseHttps();
    // });
});


PipeServer.Start();
ReceiveSensorData.Start();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapHub<AccountHub>("/accountHub");

app.MapBlazorHub(); // razor

Console.WriteLine("App runs");

app.Run();
