using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;

namespace Agencia_Viajes_ADS.Pages
{
    public class ReservasModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReservasModel(AppDbContext context)
        {
            _context = context;
        }

        // form

        [BindProperty]
        public int IdTour { get; set; }

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        public decimal Monto { get; set; }

        [BindProperty]
        public string MetodoPago { get; set; } = "";

        [BindProperty]
        public int Cuotas { get; set; } = 1;

        [BindProperty]
        public string Factura { get; set; } = "";

        // DATOS PARA VISTA

        public string NombreUsuario { get; set; } = "";

        public List<TourVm> Tours { get; set; } = new();

        public List<ClienteVm> Clientes { get; set; } = new();

        public List<ReservaVm> ReservasCliente { get; set; } = new();

        // GET

        public void OnGet()
        {
            CargarDatos();
        }

        // POST
        public IActionResult OnPost()
        {
            try
            {
                // Buscar cliente elegido en el form
                var cliente = _context.Clientes
                    .FirstOrDefault(c => c.IdCliente == IdCliente);

                if (cliente == null)
                {
                    ModelState.AddModelError("", "Cliente no encontrado");
                    CargarDatos();
                    return Page();
                }

                // Buscar tour
                var tour = _context.Tours
                    .FirstOrDefault(t => t.IdTour == IdTour);

                if (tour == null)
                {
                    ModelState.AddModelError("", "Tour no encontrado");
                    CargarDatos();
                    return Page();
                }

                // Validar cupos
                if (tour.PlazasOcupadas >= tour.CantidadPlazas)
                {
                    ModelState.AddModelError("", "No hay plazas disponibles");
                    CargarDatos();
                    return Page();
                }

                // GUARDAR PAGO

                var pago = new Pago
                {
                    MontoTotal = Monto,
                    MetodoPago = MetodoPago,
                    CantidadCuotas = Cuotas,
                    Factura = Factura,
                    FechaPago = DateTime.UtcNow
                };

                _context.Pagos.Add(pago);
                _context.SaveChanges();
                // GUARDAR INSCRIPCION


                var inscripcion = new Inscripcion
                {
                    IdCliente = cliente.IdCliente,
                    IdTour = tour.IdTour,
                    IdPago = pago.IdPago,
                    FechaInscripcion = DateTime.UtcNow,
                    Estado = "Activa"
                };

                _context.Inscripciones.Add(inscripcion);

                // Actualizar plazas
                tour.PlazasOcupadas += 1;

                _context.SaveChanges();

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                var mensajeCompleto = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", $"Error: {mensajeCompleto}");
                CargarDatos();
                return Page();
            }
        }

        // METODO CARGAR DATOS


        private void CargarDatos()
        {
            NombreUsuario = User.Identity?.Name ?? "";

            // Tours con espacio disponible
            Tours = _context.Tours
                .Where(t => t.PlazasOcupadas < t.CantidadPlazas)
                .Select(t => new TourVm
                {
                    IdTour = t.IdTour,
                    NombreTour = t.NombreTour
                })
                .ToList();

            // Lista de clientes activos
            Clientes = _context.Clientes
                .Where(c => c.EstadoCliente == true)
                .Select(c => new ClienteVm
                {
                    IdCliente = c.IdCliente,
                    Nombre = c.Nombre
                })
                .ToList();

            // Todas las reservas con nombre de cliente
            ReservasCliente = (
                from i in _context.Inscripciones
                join t in _context.Tours on i.IdTour equals t.IdTour
                join p in _context.Pagos on i.IdPago equals p.IdPago
                join c in _context.Clientes on i.IdCliente equals c.IdCliente
                orderby i.FechaInscripcion descending
                select new ReservaVm
                {
                    IdInscripcion = i.IdInscripcion,
                    NombreCliente = c.Nombre,
                    NombreTour = t.NombreTour,
                    Estado = i.Estado,
                    FechaInscripcion = i.FechaInscripcion,
                    Pago = p.MontoTotal
                }
            ).ToList();
        }


        // VIEW MODELS

        public class TourVm
        {
            public int IdTour { get; set; }
            public string NombreTour { get; set; } = "";
        }

        public class ClienteVm
        {
            public int IdCliente { get; set; }
            public string Nombre { get; set; } = "";
        }

        public class ReservaVm
        {
            public int IdInscripcion { get; set; }
            public string NombreCliente { get; set; } = "";
            public string NombreTour { get; set; } = "";
            public string Estado { get; set; } = "";
            public DateTime FechaInscripcion { get; set; }
            public decimal Pago { get; set; }
        }
    }
}