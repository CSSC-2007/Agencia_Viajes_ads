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
                Console.WriteLine("Las tablas ya existen, validando esquema...");
                
                // Migración manual para el cambio de relación Tour-Escala (1:1 -> 1:N)
                try 
                {
                    // 1. Agregar id_tour a escala si no existe
                    db.Database.ExecuteSqlRaw(@"
                        DO $$ 
                        BEGIN 
                            IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='escala' AND column_name='id_tour') THEN
                                ALTER TABLE public.escala ADD COLUMN id_tour BIGINT;
                                ALTER TABLE public.escala ADD CONSTRAINT fk_escala_tour FOREIGN KEY (id_tour) REFERENCES public.tour(id_tour) ON DELETE CASCADE;
                            END IF;
                        END $$;");

                    // 2. Eliminar id_escala de tour si existe
                    db.Database.ExecuteSqlRaw(@"
                        DO $$ 
                        BEGIN 
                            IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='tour' AND column_name='id_escala') THEN
                                ALTER TABLE public.tour DROP COLUMN id_escala;
                            END IF;
                        END $$;");

                    // 3. Agregar precio a tour si no existe
                    db.Database.ExecuteSqlRaw(@"
                        DO $$ 
                        BEGIN 
                            IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='tour' AND column_name='precio') THEN
                                ALTER TABLE public.tour ADD COLUMN precio NUMERIC(12,2) DEFAULT 0;
                            END IF;
                        END $$;");
                    
                    Console.WriteLine("Esquema actualizado correctamente.");
                }
                catch (Exception migEx)
                {
                    Console.WriteLine($"[AVISO MIGRACIÓN]: {migEx.Message}");
                }
            }
        }

        // Seed Data
        DbSeeder.Seed(db);
        Console.WriteLine("Sembrado de base de datos completado.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR CRITICO DB]: {ex.Message}");
    }
}

app.Run();
