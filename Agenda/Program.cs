using Agenda.Data;
using Agenda.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================
// MVC
// =========================
builder.Services.AddControllersWithViews();

// =========================
// DbContext SQL Server
// =========================
builder.Services.AddDbContext<AgendaDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

// =========================
// Servicios de dominio
// =========================
builder.Services.AddScoped<IAgendaService, AgendaService>();

// =========================
// Configuración fuerte
// =========================
builder.Services.Configure<AgendaSettings>(
    builder.Configuration.GetSection("AgendaSettings")
);

builder.Services.AddMemoryCache();

var app = builder.Build();

// =========================
// Pipeline
// =========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// =========================
// MIGRACIÓN + SEED (CRÍTICO)
// =========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AgendaDbContext>();

    Console.WriteLine("=====================================");
    Console.WriteLine("DB NAME     : " + db.Database.GetDbConnection().Database);
    Console.WriteLine("DB SERVER   : " + db.Database.GetDbConnection().DataSource);
    Console.WriteLine("=====================================");

    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(db);
}

// =========================
// Rutas
// =========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Agenda}/{action=Index}/{id?}");

app.Run();
