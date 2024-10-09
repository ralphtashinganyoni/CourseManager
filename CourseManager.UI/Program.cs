using CourseManager.UI.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Toast;
using Microsoft.EntityFrameworkCore;
using CourseManager.UI.Identity;
using CourseManager.UI.DataAccessLayer;
using CourseManager.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));

builder.Services.AddBlazoredToast();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAttendeeService, AttendeeService>();

builder.RootComponents.Add<HeadOutlet>("head::after");

// register settings
builder.Configuration.AddJsonFile("appsettings.json");

// register the cookie handler
builder.Services.AddScoped<CookieHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// set base address for default host
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["HostUrl"]!) });

// configure client for auth interactions
builder.Services.AddHttpClient(
    "Auth",
    opt => opt.BaseAddress = new Uri(builder.Configuration["AuthUrl"]!))
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
