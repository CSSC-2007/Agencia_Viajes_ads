using Microsoft.AspNetCore.Mvc.RazorPages;
using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Agencia_Viajes_ADS.Pages
{
    public class FinanzasModel : PageModel
    {
        private readonly AppDbContext _context;

        public FinanzasModel(AppDbContext context)
        {
            _context = context;
        }

        public decimal IngresosTotales { get; set; }
        public decimal PagosPendientes { get; set; }
        public double CrecimientoMensual { get; set; }
        public int CantidadFacturasPendientes { get; set; }

        public List<TransaccionVm> TransaccionesRecientes { get; set; } = new();
        public List<MonthlyIncomeVm> IngresosPorMes { get; set; } = new();
        public List<SalesByDestinationVm> VentasPorDestino { get; set; } = new();

        public void OnGet()
        {
            // Ingresos Totales
            IngresosTotales = _context.Pagos.Sum(p => p.MontoTotal);

            // Pagos Pendientes (Mocking logic based on "Cancelada" vs "Activa" or just showing something)
            // Since we don't have a strict 'Pendiente', we'll use a subset for demonstration or just 0
            PagosPendientes = _context.Inscripciones
                .Where(i => i.Estado == "Pendiente")
                .Join(_context.Pagos, i => i.IdPago, p => p.IdPago, (i, p) => p.MontoTotal)
                .Sum();
            
            CantidadFacturasPendientes = _context.Inscripciones.Count(i => i.Estado == "Pendiente");

            // Crecimiento Mensual (Simplified)
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            
            var currentMonthIncome = _context.Pagos
                .Where(p => p.FechaPago.Month == currentMonth && p.FechaPago.Year == currentYear)
                .Sum(p => (decimal?)p.MontoTotal) ?? 0;

            var lastMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var lastMonthYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            var lastMonthIncome = _context.Pagos
                .Where(p => p.FechaPago.Month == lastMonth && p.FechaPago.Year == lastMonthYear)
                .Sum(p => (decimal?)p.MontoTotal) ?? 0;

            if (lastMonthIncome > 0)
            {
                CrecimientoMensual = (double)((currentMonthIncome - lastMonthIncome) / lastMonthIncome) * 100;
            }
            else if (currentMonthIncome > 0)
            {
                CrecimientoMensual = 100;
            }

            // Transacciones Recientes
            TransaccionesRecientes = (
                from i in _context.Inscripciones
                join t in _context.Tours on i.IdTour equals t.IdTour
                join p in _context.Pagos on i.IdPago equals p.IdPago
                join c in _context.Clientes on i.IdCliente equals c.IdCliente
                orderby i.FechaInscripcion descending
                select new TransaccionVm
                {
                    IdPago = p.IdPago,
                    Factura = p.Factura,
                    Cliente = c.Nombre,
                    Paquete = t.NombreTour,
                    Fecha = i.FechaInscripcion,
                    Monto = p.MontoTotal,
                    Estado = i.Estado
                }
            ).Take(5).ToList();

            // Ingresos por Mes (Current Year)
            var monthlyData = _context.Pagos
                .Where(p => p.FechaPago.Year == currentYear)
                .GroupBy(p => p.FechaPago.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(p => p.MontoTotal) })
                .ToList();

            for (int i = 1; i <= 12; i++)
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i).ToUpper();
                var total = monthlyData.FirstOrDefault(m => m.Month == i)?.Total ?? 0;
                IngresosPorMes.Add(new MonthlyIncomeVm { Mes = monthName, Monto = total });
            }

            // Ventas por Destino
            VentasPorDestino = (
                from i in _context.Inscripciones
                join t in _context.Tours on i.IdTour equals t.IdTour
                group i by t.NombreTour into g
                select new SalesByDestinationVm
                {
                    Destino = g.Key,
                    Cantidad = g.Count()
                }
            ).OrderByDescending(x => x.Cantidad).Take(3).ToList();
        }

        public class TransaccionVm
        {
            public int IdPago { get; set; }
            public string Factura { get; set; } = "";
            public string Cliente { get; set; } = "";
            public string Paquete { get; set; } = "";
            public DateTime Fecha { get; set; }
            public decimal Monto { get; set; }
            public string Estado { get; set; } = "";
        }

        public class MonthlyIncomeVm
        {
            public string Mes { get; set; } = "";
            public decimal Monto { get; set; }
        }

        public class SalesByDestinationVm
        {
            public string Destino { get; set; } = "";
            public int Cantidad { get; set; }
        }
    }
}