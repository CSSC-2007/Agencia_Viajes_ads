using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Pages
{
    public class ClientesModel : PageModel
    {
        private readonly AppDbContext _db;

        public ClientesModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Cliente> Clientes { get; set; } = new();

        [BindProperty]
        public Cliente ClienteForm { get; set; } = new();

        public string? MensajeError { get; set; }

        public async Task OnGetAsync()
        {
            Clientes = await _db.Clientes
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCrearAsync()
        {
            if (await _db.Clientes.AnyAsync(c => c.IdCliente == ClienteForm.IdCliente))
            {
                ModelState.AddModelError("ClienteForm.IdCliente", "El ID del Cliente ya existe.");
            }

            if (!ModelState.IsValid)
            {
                Clientes = await _db.Clientes.OrderBy(c => c.Nombre).ToListAsync();
                MensajeError = "Por favor corrige los errores del formulario.";
                return Page();
            }
            _db.Clientes.Add(ClienteForm);
            await _db.SaveChangesAsync();
            TempData["Exito"] = "Cliente creado exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditarAsync()
        {
            if (!ModelState.IsValid)
            {
                Clientes = await _db.Clientes.OrderBy(c => c.Nombre).ToListAsync();
                MensajeError = "Por favor corrige los errores del formulario.";
                return Page();
            }
            var cliente = await _db.Clientes.FindAsync(ClienteForm.IdCliente);
            if (cliente == null) return NotFound();

            cliente.Nombre = ClienteForm.Nombre;
            cliente.Direccion = ClienteForm.Direccion;
            cliente.Telefono = ClienteForm.Telefono;
            cliente.Ocupacion = ClienteForm.Ocupacion;

            await _db.SaveChangesAsync();
            TempData["Exito"] = "Cliente actualizado exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(int id)
        {
            var cliente = await _db.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            cliente.EstadoCliente = !cliente.EstadoCliente;
            await _db.SaveChangesAsync();

            TempData["Exito"] = cliente.EstadoCliente ? "Cliente activado." : "Cliente desactivado.";
            return RedirectToPage();
        }
    }
}