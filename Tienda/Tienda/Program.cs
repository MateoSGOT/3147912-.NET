using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Tienda.Data;
using Tienda.Models;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   // ?? Necesario para las p�ginas de Identity

// DbContext de la tienda
builder.Services.AddDbContext<TechNovaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// DbContext de Identity
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Identity (login / registro)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AuthDbContext>();

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

// ?? IMPORTANT�SIMO: primero autenticaci�n, luego autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ?? Aqu� se mapean las p�ginas de Identity (Login, Register, etc.)
app.MapRazorPages();

app.Run();
