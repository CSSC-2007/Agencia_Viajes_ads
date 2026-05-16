using Agencia_Viajes_ADS.Data;
using Agencia_Viajes_ADS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Agencia_Viajes_ADS.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "El nombre de usuario es requerido.")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 30 caracteres.")]
            [Display(Name = "Usuario")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es requerida.")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userExists = await _context.Usuarios.AnyAsync(u => u.Username == Input.Username);
                if (userExists)
                {
                    ModelState.AddModelError("Input.Username", "El nombre de usuario ya está en uso.");
                    return Page();
                }

                // Default to 'Turismo' role for new registrations
                var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Turismo");
                if (defaultRole == null)
                {
                    ModelState.AddModelError(string.Empty, "Error en la configuración de roles. Contacte al administrador.");
                    return Page();
                }

                var user = new Usuario
                {
                    Username = Input.Username,
                    PasswordHash = Input.Password, // Storing as plain text per request
                    IdRol = defaultRole.IdRol,
                    Activo = true
                };

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cuenta creada exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
