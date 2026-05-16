using Microsoft.AspNetCore.Mvc.RazorPages;
using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public decimal VentasHoy { get; set; }
        public int NuevosClientes { get; set; }
        public int ReservasActivas { get; set; }
        public int ToursProximos { get; set; }

        public List<InscripcionVm> UltimasInscripciones { get; set; } = new();

        public void OnGet()
        {
            var today = DateTime.UtcNow.Date;

            // Ventas Hoy
            VentasHoy = _context.Pagos
                .Where(p => p.FechaPago.Date == today)
                .Sum(p => (decimal?)p.MontoTotal) ?? 0;

            // Clientes Activos (as substitute for "new" since we lack creation date)
            NuevosClientes = _context.Clientes.Count(c => c.EstadoCliente);

            // Reservas Activas
            ReservasActivas = _context.Inscripciones.Count(i => i.Estado == "Activa");

            // Tours Próximos
            ToursProximos = _context.Tours.Count(t => t.FechaSalida > DateTime.UtcNow);

            // Últimas Inscripciones
            UltimasInscripciones = (
                from i in _context.Inscripciones
                join t in _context.Tours on i.IdTour equals t.IdTour
                join p in _context.Pagos on i.IdPago equals p.IdPago
                join c in _context.Clientes on i.IdCliente equals c.IdCliente
                orderby i.FechaInscripcion descending
                select new InscripcionVm
                {
                    Cliente = c.Nombre,
                    Tour = t.NombreTour,
                    Fecha = i.FechaInscripcion,
                    Estado = i.Estado,
                    Monto = p.MontoTotal
                }
            ).Take(5).ToList();
        }

        public class InscripcionVm
        {
            public string Cliente { get; set; } = "";
            public string Tour { get; set; } = "";
            public DateTime Fecha { get; set; }
            public string Estado { get; set; } = "";
            public decimal Monto { get; set; }
        }
    }
}