using Lab2.RezervacijeProstora;
using Lab2.RezervacijeProstora.Data;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Registriraj ApplicationDbContext u dependency injection container i koristi SQLite bazu iz connection stringa
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    builder.Services
        .AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = googleClientId;
            options.ClientSecret = googleClientSecret;
        });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context, scope.ServiceProvider);
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "prostori",
    pattern: "prostori",
    defaults: new { controller = "ProstorZaProbu", action = "Index" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "prostor_detalji",
    pattern: "prostori/detalji/{id:int}",
    defaults: new { controller = "ProstorZaProbu", action = "Details" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "rezervacije",
    pattern: "rezervacije",
    defaults: new { controller = "Rezervacija", action = "Index" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "korisnik_detalji",
    pattern: "korisnici/detalji/{id:int}",
    defaults: new { controller = "Korisnik", action = "Details" })
    .WithStaticAssets();

//Default ruta ima tri dijela: controller, action i opcionalni id
//Ako nešto nije navedeno, koriste se zadane vrijednosti Home i Index.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

public partial class Program
{
}
