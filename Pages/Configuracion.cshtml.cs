using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public int MinViajesReporte { get; set; } = 3;
        public List<MetodoPago> MetodosPago { get; set; } = new();

        [BindProperty]
        public ChangePasswordInputModel PasswordInput { get; set; } = new();

        [BindProperty]
        public UpdateUserInputModel UpdateInput { get; set; } = new();

        [BindProperty]
        public EscalaInputModel EscalaInput { get; set; } = new();

        [BindProperty]
        public MetodoPagoInputModel MetodoInput { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
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
                    IdUsuario = u.IdUsuario,
                    Username = u.Username,
                    IdRol = u.IdRol,
                    Rol = u.Rol.NombreRol,
                    Activo = u.Activo
                }).ToListAsync();

            Roles = await _context.Roles.ToListAsync();
            Escalas = await _context.Escalas.OrderBy(e => e.Orden).ToListAsync();
            MetodosPago = await _context.MetodosPago.OrderBy(m => m.NombreMetodo).ToListAsync();
        }

        public async Task<IActionResult> OnPostAddMetodoAsync()
        {
            var prefix = nameof(MetodoInput);
            var errors = ModelState.Where(m => m.Key.StartsWith(prefix))
                                   .SelectMany(m => m.Value.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            if (errors.Any()) 
                return new JsonResult(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });

            var nuevoMetodo = new MetodoPago { NombreMetodo = MetodoInput.NombreMetodo };
            _context.MetodosPago.Add(nuevoMetodo);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Método de pago agregado." });
        }

        public async Task<IActionResult> OnPostDeleteMetodoAsync([FromQuery] int id)
        {
            var metodo = await _context.MetodosPago.FindAsync(id);
            if (metodo == null) return new JsonResult(new { success = false, message = "Método no encontrado." });

            _context.MetodosPago.Remove(metodo);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Método de pago eliminado." });
        }

        public async Task<IActionResult> OnPostAddEscalaAsync()
        {
            // Only validate EscalaInput prefix
            var prefix = nameof(EscalaInput);
            var errors = ModelState.Where(m => m.Key.StartsWith(prefix))
                                   .SelectMany(m => m.Value.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            if (errors.Any()) 
            {
                return new JsonResult(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            var nuevaEscala = new Escala
            {
                LugarEscala = EscalaInput.LugarEscala,
                Orden = EscalaInput.Orden
            };

            _context.Escalas.Add(nuevaEscala);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Escala agregada correctamente." });
        }

        public async Task<IActionResult> OnPostEditEscalaAsync()
        {
            var prefix = nameof(EscalaInput);
            var errors = ModelState.Where(m => m.Key.StartsWith(prefix))
                                   .SelectMany(m => m.Value.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            if (errors.Any())
            {
                return new JsonResult(new { success = false, message = "Datos inválidos: " + string.Join(", ", errors) });
            }

            if (EscalaInput.IdEscala == 0) 
                return new JsonResult(new { success = false, message = "ID de escala inválido." });

            var escala = await _context.Escalas.FindAsync(EscalaInput.IdEscala);
            if (escala == null) return new JsonResult(new { success = false, message = "Escala no encontrada." });

            escala.LugarEscala = EscalaInput.LugarEscala;
            escala.Orden = EscalaInput.Orden;

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Escala actualizada correctamente." });
        }

        public async Task<IActionResult> OnPostDeleteEscalaAsync([FromQuery] int id)
        {
            try 
            {
                var escala = await _context.Escalas.FindAsync(id);
                if (escala == null) return new JsonResult(new { success = false, message = "Escala no encontrada." });

                // Check if it's used in any Tour
                var isUsed = escala.IdTour != 0;
                if (isUsed)
                {
                    return new JsonResult(new { success = false, message = "No se puede eliminar: Esta escala está asociada a un tour." });
                }

                _context.Escalas.Remove(escala);
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Escala eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var prefix = nameof(PasswordInput);
            var errors = ModelState.Where(m => m.Key.StartsWith(prefix))
                                   .SelectMany(m => m.Value.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();

            if (errors.Any())
            {
                return new JsonResult(new { success = false, message = string.Join(" ", errors) });
            }

            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return new JsonResult(new { success = false, message = "Sesión expirada." });

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return new JsonResult(new { success = false, message = "Usuario no encontrado." });

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(PasswordInput.CurrentPassword, user.PasswordHash))
            {
                return new JsonResult(new { success = false, message = "La contraseña actual es incorrecta." });
            }

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(PasswordInput.NewPassword);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, message = "Contraseña actualizada correctamente." });
        }

        public async Task<IActionResult> OnPostUpdateUserAsync()
        {
            // UpdateUser uses primitive fields and hidden inputs, 
            // no complex validation needed here besides the existence of the user.
            
            // Re-verify role from DB to be absolutely sure
            var currentUser = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (currentUser?.Rol?.NombreRol != "Gerente")
            {
                return new JsonResult(new { success = false, message = "No tiene permisos para realizar esta acción. Solo el Gerente puede editar usuarios." });
            }

            var userToUpdate = await _context.Usuarios.FindAsync(UpdateInput.IdUsuario);
            if (userToUpdate == null) return new JsonResult(new { success = false, message = "Usuario no encontrado." });

            userToUpdate.IdRol = UpdateInput.IdRol;
            userToUpdate.Activo = UpdateInput.Activo;

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Usuario actualizado correctamente." });
        }

        public class UpdateUserInputModel
        {
            public int IdUsuario { get; set; }
            public int IdRol { get; set; }
            public bool Activo { get; set; }
        }

        public class EscalaInputModel
        {
            public int IdEscala { get; set; }
            [Required(ErrorMessage = "El lugar de la escala es obligatorio")]
            public string LugarEscala { get; set; } = "";
            [Required(ErrorMessage = "El orden es obligatorio")]
            public int Orden { get; set; }
        }

        public class MetodoPagoInputModel
        {
            [Required(ErrorMessage = "El nombre del método es obligatorio")]
            public string NombreMetodo { get; set; } = "";
        }

        public class ChangePasswordInputModel
        {
            [Required(ErrorMessage = "La contraseña actual es obligatoria")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña Actual")]
            public string CurrentPassword { get; set; } = "";

            [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva Contraseña")]
            public string NewPassword { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Nueva Contraseña")]
            [Compare("NewPassword", ErrorMessage = "La nueva contraseña y su confirmación no coinciden")]
            public string ConfirmPassword { get; set; } = "";
        }

        public class UsuarioVm
        {
            public int IdUsuario { get; set; }
            public string Username { get; set; } = "";
            public int IdRol { get; set; }
            public string Rol { get; set; } = "";
            public bool Activo { get; set; }
        }
    }
}