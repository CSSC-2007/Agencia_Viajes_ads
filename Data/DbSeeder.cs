using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Roles.Any())
            {
                db.Roles.AddRange(
                    new Rol { NombreRol = "Gerente" },
                    new Rol { NombreRol = "Atencion" },
                    new Rol { NombreRol = "Turismo" }
                );
                db.SaveChanges();
            }

            var gerenteRole = db.Roles.First(r => r.NombreRol == "Gerente");
            var atencionRole = db.Roles.First(r => r.NombreRol == "Atencion");
            var turismoRole = db.Roles.First(r => r.NombreRol == "Turismo");

            if (!db.Usuarios.Any())
            {
                db.Usuarios.AddRange(
                    new Usuario { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), IdRol = gerenteRole.IdRol },
                    new Usuario { Username = "atencion", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atencion123!"), IdRol = atencionRole.IdRol },
                    new Usuario { Username = "turismo", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Turismo123!"), IdRol = turismoRole.IdRol }
                );
                db.SaveChanges();
            }

            if (!db.Clientes.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    db.Clientes.Add(new Cliente
                    {
                        Nombre = $"Cliente Test {i}",
                        Direccion = $"Calle {i}, Ciudad",
                        Telefono = $"7000-00{i:D2}",
                        Ocupacion = "Turista",
                        EstadoCliente = true
                    });
                }
                db.SaveChanges();
            }

            if (!db.Tours.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    var tour = new Tour
                    {
                        NombreTour = $"Tour Destino {i}",
                        DescripcionTour = $"Descripción increíble del tour {i}",
                        FechaSalida = DateTime.UtcNow.AddDays(i * 2),
                        FechaLlegada = DateTime.UtcNow.AddDays(i * 2 + 5),
                        CantidadPlazas = 10 + i,
                        PlazasOcupadas = 0,
                        Precio = 100 * i
                    };
                    
                    tour.Escalas.Add(new Escala { LugarEscala = "Origen", Orden = 1 });
                    tour.Escalas.Add(new Escala { LugarEscala = $"Escala {i}", Orden = 2 });
                    tour.Escalas.Add(new Escala { LugarEscala = "Destino Final", Orden = 3 });

                    db.Tours.Add(tour);
                }
                db.SaveChanges();
            }

            // Inyectar 20 inscripciones y pagos para los reportes
            if (!db.Inscripciones.Any())
            {
                var clientes = db.Clientes.ToList();
                var tours = db.Tours.ToList();

                for (int i = 0; i < 20; i++)
                {
                    var cliente = clientes[i % 20];
                    var tour = tours[i % 20];

                    var pago = new Pago
                    {
                        MontoTotal = 500 + (i * 50),
                        MetodoPago = i % 3 == 0 ? "Cuotas" : (i % 3 == 1 ? "Tarjeta" : "Efectivo"),
                        CantidadCuotas = i % 3 == 0 ? 3 : 1,
                        FechaPago = DateTime.UtcNow,
                        Factura = $"FAC-{1000 + i}"
                    };
                    db.Pagos.Add(pago);
                    db.SaveChanges();

                    db.Inscripciones.Add(new Inscripcion
                    {
                        IdCliente = cliente.IdCliente,
                        IdTour = tour.IdTour,
                        IdPago = pago.IdPago,
                        FechaInscripcion = DateTime.UtcNow,
                        Estado = "Activa"
                    });
                    tour.PlazasOcupadas += 1;
                }
                db.SaveChanges();
            }
        }
    }
}