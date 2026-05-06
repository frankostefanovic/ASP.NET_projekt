using Lab2.RezervacijeProstora;
using Lab2.RezervacijeProstora.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
