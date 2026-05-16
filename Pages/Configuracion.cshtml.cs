using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;

namespace Agencia_Viajes_ADS.Pages
{
    public class ConfiguracionModel : PageModel
    {
        private readonly AppDbContext _context;

        public ConfiguracionModel(AppDbContext context)
        {
            _context = context;
        }

        public string CurrentUsername { get; set; } = "";
        public string CurrentRole { get; set; } = "";

        public List<UsuarioVm> Usuarios { get; set; } = new();
        public List<Rol> Roles { get; set; } = new();
        public List<Escala> Escalas { get; set; } = new();

        public async Task OnGetAsync()
        {
            CurrentUsername = User.Identity?.Name ?? "Usuario";
            
            // Get current user role
            var user = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == CurrentUsername);
            
            CurrentRole = user?.Rol?.NombreRol ?? "Sin Rol";

            // Load data for management
            Usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Select(u => new UsuarioVm {
                    Username = u.Username,
                    Rol = u.Rol.NombreRol,
                    Activo = u.Activo
                }).ToListAsync();

            Roles = await _context.Roles.ToListAsync();
            Escalas = await _context.Escalas.OrderBy(e => e.Orden).ToListAsync();
        }

        public class UsuarioVm
        {
            public string Username { get; set; } = "";
            public string Rol { get; set; } = "";
            public bool Activo { get; set; }
        }
    }
}