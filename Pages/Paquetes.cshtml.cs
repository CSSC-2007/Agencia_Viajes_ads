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

        public List<SelectListItem> EscalasDisponibles { get; set; } = new();

        [BindProperty]
        public Tour TourForm { get; set; } = new();

        public async Task OnGetAsync()
        {
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            Paquetes = await _db.Tours
                .Include(t => t.Escala)
                .OrderBy(t => t.IdTour)
                .ToListAsync();

            EscalasDisponibles = await _db.Escalas
                .OrderBy(e => e.Orden)
                .Select(e => new SelectListItem
                {
                    Value = e.IdEscala.ToString(),
                    Text = e.LugarEscala + " - Orden " + e.Orden
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCrearAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarDatos();
                return Page();
            }

            _db.Tours.Add(TourForm);

            await _db.SaveChangesAsync();

            TempData["Exito"] = "Paquete creado correctamente.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarAsync()
        {
            var paquete = await _db.Tours.FindAsync(TourForm.IdTour);

            if (paquete == null)
            {
                return NotFound();
            }

            paquete.IdEscala = TourForm.IdEscala;
            paquete.NombreTour = TourForm.NombreTour;
            paquete.DescripcionTour = TourForm.DescripcionTour;
            paquete.FechaSalida = TourForm.FechaSalida;
            paquete.FechaLlegada = TourForm.FechaLlegada;
            paquete.CantidadPlazas = TourForm.CantidadPlazas;
            paquete.PlazasOcupadas = TourForm.PlazasOcupadas;

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