using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Pages
{
    public class FinanzasModel : PageModel
    {
        private readonly AppDbContext _context;

        public FinanzasModel(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // VARIABLES DASHBOARD
        // =========================

        public decimal TotalIngresos { get; set; }

        public int PagosCuotas { get; set; }

        public int FacturasGeneradas { get; set; }

        public int ToursVendidos { get; set; }

        public int PagosPendientes { get; set; }

        public int CuotasPendientes { get; set; }

        public List<int> IngresosMensuales { get; set; } = new();

        public List<Inscripcion> UltimosPagos { get; set; } = new();

        // =========================
        // FORMULARIO
        // =========================

        [BindProperty]
        public Pago NuevoPago { get; set; } = new();

        // =========================
        // LOAD PAGE
        // =========================

        public async Task OnGetAsync()
        {
            await CargarDashboard();
        }

        // =========================
        // GUARDAR PAGO
        // =========================

        public async Task<IActionResult> OnPostRegistrarPagoAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarDashboard();
                return Page();
            }

            NuevoPago.FechaPago = DateTime.Now;

            _context.Pagos.Add(NuevoPago);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // =========================
        // METODO DASHBOARD
        // =========================

        private async Task CargarDashboard()
        {
            // TOTAL INGRESOS
            TotalIngresos = await _context.Pagos
                .SumAsync(p => (decimal?)p.MontoTotal) ?? 0;

            // PAGOS EN CUOTAS
            PagosCuotas = await _context.Pagos
                .CountAsync(p => p.CantidadCuotas > 1);

            // FACTURAS
            FacturasGeneradas = await _context.Pagos
                .CountAsync();

            // TOURS VENDIDOS
            ToursVendidos = await _context.Inscripciones
                .CountAsync();

            // PAGOS PENDIENTES
            PagosPendientes = await _context.Inscripciones
                .CountAsync(i => i.Estado == "Pendiente");

            // CUOTAS
            CuotasPendientes = await _context.Pagos
                .CountAsync(p => p.CantidadCuotas > 1);

            // TABLA
            UltimosPagos = await _context.Inscripciones
                .Include(i => i.Cliente)
                .Include(i => i.Pago)
                .OrderByDescending(i => i.FechaInscripcion)
                .Take(5)
                .ToListAsync();

            // GRAFICA
            IngresosMensuales = new List<int>();

            for (int mes = 1; mes <= 12; mes++)
            {
                var totalMes = await _context.Pagos
                    .Where(p => p.FechaPago.Month == mes)
                    .SumAsync(p => (decimal?)p.MontoTotal) ?? 0;

                IngresosMensuales.Add((int)totalMes);
            }
        }
    }
}