using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Pages
{
    public class PaquetesModel : PageModel
    {
        private readonly AppDbContext _db;

        public PaquetesModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Tour> Paquetes { get; set; } = new();

        [BindProperty]
        public Tour TourForm { get; set; } = new();

        [BindProperty]
        public string EscalasTexto { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            Paquetes = await _db.Tours
                .Include(t => t.Escalas)
                .OrderBy(t => t.IdTour)
                .ToListAsync();
        }

        private DateTime ConvertirUtc(DateTime fecha)
        {
            if (fecha.Kind == DateTimeKind.Utc)
                return fecha;

            return DateTime.SpecifyKind(fecha, DateTimeKind.Utc);
        }

        public async Task<IActionResult> OnPostCrearAsync()
        {
            // Validar si el ID ya existe
            if (await _db.Tours.AnyAsync(t => t.IdTour == TourForm.IdTour))
            {
                ModelState.AddModelError("TourForm.IdTour", "El ID del Tour ya existe. Por favor use uno diferente.");
            }

            // Validaciones de fecha
            if (TourForm.FechaSalida < DateTime.Now.Date)
            {
                ModelState.AddModelError("TourForm.FechaSalida", "La fecha de salida no puede ser anterior a hoy.");
            }

            if (TourForm.FechaLlegada < TourForm.FechaSalida)
            {
                ModelState.AddModelError("TourForm.FechaLlegada", "La fecha de llegada no puede ser anterior a la fecha de salida.");
            }

            if (!ModelState.IsValid)
            {
                await CargarDatos();
                return Page();
            }

            TourForm.FechaSalida = ConvertirUtc(TourForm.FechaSalida);
            TourForm.FechaLlegada = ConvertirUtc(TourForm.FechaLlegada);

            // Procesar escalas si se enviaron (separadas por coma)
            if (!string.IsNullOrWhiteSpace(EscalasTexto))
            {
                var nombres = EscalasTexto.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < nombres.Length; i++)
                {
                    TourForm.Escalas.Add(new Escala 
                    { 
                        LugarEscala = nombres[i].Trim(),
                        Orden = i + 1
                    });
                }
            }

            _db.Tours.Add(TourForm);
            await _db.SaveChangesAsync();

            TempData["Exito"] = "Paquete creado correctamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarAsync()
        {
            if (TourForm.FechaLlegada < TourForm.FechaSalida)
            {
                ModelState.AddModelError("TourForm.FechaLlegada", "La fecha de llegada no puede ser anterior a la fecha de salida.");
            }

            if (!ModelState.IsValid)
            {
                await CargarDatos();
                return Page();
            }

            var paquete = await _db.Tours
                .Include(t => t.Escalas)
                .FirstOrDefaultAsync(t => t.IdTour == TourForm.IdTour);

            if (paquete == null) return NotFound();

            paquete.NombreTour = TourForm.NombreTour;
            paquete.DescripcionTour = TourForm.DescripcionTour;
            paquete.FechaSalida = ConvertirUtc(TourForm.FechaSalida);
            paquete.FechaLlegada = ConvertirUtc(TourForm.FechaLlegada);
            paquete.CantidadPlazas = TourForm.CantidadPlazas;
            paquete.PlazasOcupadas = TourForm.PlazasOcupadas;
            paquete.Precio = TourForm.Precio;

            // Actualizar escalas
            _db.Escalas.RemoveRange(paquete.Escalas);
            if (!string.IsNullOrWhiteSpace(EscalasTexto))
            {
                var nombres = EscalasTexto.Split(',', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < nombres.Length; i++)
                {
                    paquete.Escalas.Add(new Escala 
                    { 
                        LugarEscala = nombres[i].Trim(),
                        Orden = i + 1
                    });
                }
            }

            await _db.SaveChangesAsync();

            TempData["Exito"] = "Paquete actualizado correctamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var paquete = await _db.Tours.FindAsync(id);

            if (paquete == null)
            {
                return NotFound();
            }

            _db.Tours.Remove(paquete);

            await _db.SaveChangesAsync();

            TempData["Exito"] = "Paquete eliminado correctamente.";

            return RedirectToPage();
        }
    }
}