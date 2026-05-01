using Microsoft.EntityFrameworkCore;
using Agencia_Viajes_ADS.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();

    try 
    {
        Console.WriteLine("Iniciando validación de base de datos...");
        
        var databaseCreator = db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        
        if (databaseCreator != null)
        {
            if (!databaseCreator.Exists()) databaseCreator.Create();
            
            try 
            {
                databaseCreator.CreateTables();
                Console.WriteLine("Tablas creadas exitosamente.");
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P07") // duplicate_table
            {
                Console.WriteLine("Las tablas ya existen, saltando creación.");
            }
        }
// ... resto del seed

        // Seed Data
        if (!db.Roles.Any())
        {
            Console.WriteLine("Sembrando roles...");
            db.Roles.AddRange(
                new Agencia_Viajes_ADS.Models.Rol { NombreRol = "Gerente" },
                new Agencia_Viajes_ADS.Models.Rol { NombreRol = "Atencion" },
                new Agencia_Viajes_ADS.Models.Rol { NombreRol = "Turismo" }
            );
            db.SaveChanges();
        }

        if (!db.Usuarios.Any())
        {
            Console.WriteLine("Sembrando usuario admin...");
            var adminRole = db.Roles.First(r => r.NombreRol == "Gerente");
            db.Usuarios.Add(new Agencia_Viajes_ADS.Models.Usuario
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                IdRol = adminRole.IdRol,
                Activo = true
            });
            db.SaveChanges();
            Console.WriteLine(">>> SEMILLA LISTA: admin / Admin123!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR CRITICO DB]: {ex.Message}");
    }
}

app.Run();
